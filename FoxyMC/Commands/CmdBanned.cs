namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;

    public class CmdBanned : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/banned - Lists all banned names.");
        }

        public override void Use(Player p, string message)
        {
            List<string> plys = Server.ranks.AllGroup("banned");
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
                p.SendMessage(string.Concat(new object[] { plys.Count, " player", (plys.Count != 1) ? "s" : "", " &8banned&e: ", message.Remove(0, 2), "." }));
            }
            else
            {
                p.SendMessage("Nobody is banned.");
            }
        }

        public override string name
        {
            get
            {
                return "banned";
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

