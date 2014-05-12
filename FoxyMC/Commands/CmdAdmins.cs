namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;

    public class CmdAdmins : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/admins - List the admins of the server");
        }

        public override void Use(Player p, string message)
        {
            List<string> plys = Server.ranks.AllGroup("superop");
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
                p.SendMessage(string.Concat(new object[] { plys.Count, " &4Admin", (plys.Count != 1) ? "s" : "", "&e: ", message.Remove(0, 2), "." }));
            }
            else
            {
                p.SendMessage("Nobody is admin. What's wrong with this server?");
            }
        }

        public override string name
        {
            get
            {
                return "admins";
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

