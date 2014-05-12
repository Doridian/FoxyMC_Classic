namespace FoxyMC.Commands
{
    using System;

    public class CmdMe : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/me <action> - Roleplay-like action message.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else if (Server.worldChat)
            {
                Player.GlobalChat(p, p.color + "*" + p.name + " " + message, false);
            }
            else
            {
                Player.GlobalChatLevel(p, p.color + "*" + p.name + " " + message, false);
            }
        }

        public override string name
        {
            get
            {
                return "me";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Guest;
            }
        }
    }
}

