namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;

    public class CmdBannedip : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/bannedip - Lists all banned IPs.");
        }

        public override void Use(Player p, string message)
        {
            List<string> plys = Server.bannedIP.All();
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
                p.SendMessage(plys.Count.ToString() + " IP" + ((plys.Count != 1) ? "s" : "") + "&8banned&e: " + message.Remove(0, 2) + ".");
            }
            else
            {
                p.SendMessage("No IP is banned.");
            }
        }

        public override string name
        {
            get
            {
                return "bannedip";
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

