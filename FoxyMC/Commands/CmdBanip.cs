namespace FoxyMC.Commands
{
    using System;
    using System.Text.RegularExpressions;

    public class CmdBanip : Command
    {
        private Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

        public override void Help(Player p)
        {
            p.SendMessage("/banip <ip/name> - Bans an ip, can also use the name of an online player.");
            p.SendMessage(" -Kicks players with matching ip as well.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                if (p != null)
                {
                    this.Help(p);
                }
            }
            else
            {
                Player player = null;
                player = Player.Find(message);
                if (player != null)
                {
                    message = player.ip;
                }
                if (message.Equals("127.0.0.1"))
                {
                    if (p != null)
                    {
                        p.SendMessage("You can't ip-ban the server!");
                    }
                }
                else if (!this.regex.IsMatch(message))
                {
                    if (p != null)
                    {
                        p.SendMessage("Not a valid ip!");
                    }
                }
                else if ((p != null) && (p.ip == message))
                {
                    p.SendMessage("You can't ip-ban yourself.!");
                }
                else if (Server.bannedIP.Contains(message))
                {
                    if (p != null)
                    {
                        p.SendMessage(message + " is already ip-banned.");
                    }
                }
                else
                {
                    Player.GlobalMessage(message + " got &8ip-banned&e!");
                    Server.bannedIP.Add(message);
                    Server.bannedIP.Save("banned-ip.txt", false);
                    Server.s.Log("IP-BANNED: " + message.ToLower());
                    foreach (Player player2 in Player.players)
                    {
                        if (message.Equals(player2.ip))
                        {
                            player2.Kick("Kicked by ipban");
                        }
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "banip";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Operator;
            }
        }
    }
}

