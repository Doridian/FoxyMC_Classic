namespace FoxyMC.Commands
{
    using System;

    public class CmdSolid : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/solid - Turns solid mode on/off.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                if (p.BlockAction == 1)
                {
                    p.BlockAction = 0;
                    p.SendMessage("Solid mode: &cOFF&e.");
                }
                else
                {
                    p.BlockAction = 1;
                    p.SendMessage("Solid Mode: &aON&e.");
                }
                p.painting = false;
            }
        }

        public override string name
        {
            get
            {
                return "solid";
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

