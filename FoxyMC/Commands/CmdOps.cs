namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;

    public class CmdOps : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/ops - Lists all operators.");
        }

        public override void Use(Player p, string message)
        {
            List<string> plys = Server.ranks.AllGroup("operator");
            if (message != "")
            {
                this.Help(p);
            }

            else if (plys.Count > 0)
            {
                foreach (string name in plys)
                {
                    message = message + ", " + name;
                }
                p.SendMessage(string.Concat(new object[] { plys.Count, " &3operator", (plys.Count != 1) ? "s" : "", "&e: ", message.Remove(0, 2), "." }));
            }
            else
            {
                p.SendMessage("Nobody is op.");
            }
        }

        public override string name
        {
            get
            {
                return "ops";
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

