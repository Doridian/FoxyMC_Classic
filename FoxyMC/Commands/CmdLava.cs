namespace FoxyMC.Commands
{
    using System;

    public class CmdLava : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/lava - Turns inactive lava mode on/off.");
        }

        public override void Use(Player p, string message)
        {
            if (p.BlockAction == 2)
            {
                p.BlockAction = 0;
                p.SendMessage("Lava mode: &cOFF&e.");
            }
            else
            {
                p.BlockAction = 2;
                p.SendMessage("Lava Mode: &aON&e.");
            }
            p.painting = false;
        }

        public override string name
        {
            get
            {
                return "lava";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Builder;
            }
        }
    }
}

