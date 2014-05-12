namespace FoxyMC.Commands
{
    using System;

    public class CmdWater : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/water - Turns inactive water mode on/off.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                if (p.BlockAction == 3)
                {
                    p.BlockAction = 0;
                    p.SendMessage("Water mode: &cOFF&e.");
                }
                else
                {
                    p.BlockAction = 3;
                    p.SendMessage("Water Mode: &aON&e.");
                }
                p.painting = false;
            }
        }

        public override string name
        {
            get
            {
                return "water";
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

