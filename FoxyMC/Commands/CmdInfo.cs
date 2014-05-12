namespace FoxyMC.Commands
{
    using System;

    public class CmdInfo : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/info - Displays the server information.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                p.SendMessage("FoxyMC version "+Server.Version+" by Doridian (based on MCSharp)");
            }
        }

        public override string name
        {
            get
            {
                return "info";
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

