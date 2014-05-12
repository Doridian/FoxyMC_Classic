namespace FoxyMC.Commands
{
    using System;

    public class CmdWhois : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/whois [player] - Displays information about someone.");
        }

        public override void Use(Player p, string message)
        {
            Player player = null;
            if (message == "")
            {
                player = p;
            }
            else
            {
                player = Player.Find(message);
            }
            if (player != null)
            {
                if (player == p)
                {
                    message = "&eYou are " + player.group.GetTag() + ".";
                }
                else
                {
                    message = player.color + player.name + "&e is " + player.group.GetTag() + " on &b" + player.level.name + "&e.";
                }
                if (Server.afkset.Contains(player.name))
                {
                    message = message + "-AFK-";
                }
                if (p.group.Permission >= LevelPermission.Operator)
                {
                    message = message + " IP: " + player.ip + ".";
                }
                p.SendChat(p, message);
            }
            else
            {
				p.SendMessage("There is no player \"" + message + "\"!");
            }
        }

        public override string name
        {
            get
            {
                return "whois";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Banned;
            }
        }
    }
}

