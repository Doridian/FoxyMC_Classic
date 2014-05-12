namespace FoxyMC.Commands
{
    using System;

    internal class CmdSay : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/say - brodcasts a global message to everyone in the server.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                message = "&e" + message;
                message = message.Replace("%", "&");
                Player.GlobalChat(p, message, false);
                message = message.Replace("&", "\x0003");
            }
        }

        public override string name
        {
            get
            {
                return "say";
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

