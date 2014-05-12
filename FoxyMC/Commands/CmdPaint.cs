namespace FoxyMC.Commands
{
    using System;

    public class CmdPaint : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/paint - Turns painting mode on/off.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                p.painting = !p.painting;
                if (p.painting)
                {
                    p.SendMessage("Painting mode: &aON&e.");
                }
                else
                {
                    p.SendMessage("Painting mode: &cOFF&e.");
                }
                p.BlockAction = 0;
            }
        }

        public override string name
        {
            get
            {
                return "paint";
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

