namespace FoxyMC.Commands
{
    using System;

    public class CmdActiveLava : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/activelava - Place a single block of active lava.");
        }

        public override void Use(Player p, string message)
        {
            if (p.BlockAction == 4)
            {
                p.BlockAction = 0;
                p.SendMessage("Active lava aborted.");
            }
            else
            {
                p.BlockAction = 4;
                p.SendMessage("Now placing &cactive_lava&e!");
            }
            p.painting = false;
        }

        public override string name
        {
            get
            {
                return "activelava";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.AdvBuilder;
            }
        }
    }
}

