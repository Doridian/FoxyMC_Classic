namespace FoxyMC.Commands
{
    using System;

    public class CmdWhowas : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/whowas <name> - Displays information about someone who left.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                Player player = Player.Find(message);
                if (player != null)
                {
                    p.SendChat(p, player.color + player.name + "&e is online, use /whois instead.");
                }
                else if (!Player.left.ContainsKey(message.ToLower()))
                {
                    p.SendMessage("No entry found for \"" + message + "\".");
                }
                else
                {
                    string name = message.ToLower();
                    string str2 = Player.left[name];
                    message = "&e" + name + " is " + Player.GetColor(name) + Player.GetGroup(name).name + "&e.";
                    if ((p.group == Group.Find("operator")) || (p.group == Group.Find("superOp")))
                    {
                        message = message + " IP: " + str2 + ".";
                    }
                    p.SendChat(p, message);
                }
            }
        }

        public override string name
        {
            get
            {
                return "whowas";
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

