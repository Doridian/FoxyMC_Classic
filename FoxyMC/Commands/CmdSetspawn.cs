namespace FoxyMC.Commands
{
    using System;

    public class CmdSetspawn : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/setspawn - Set the default spawn location.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                p.SendMessage("Spawn location changed.");
                p.level.spawnx = (ushort) (p.pos[0] / 0x20);
                p.level.spawny = (ushort) (p.pos[1] / 0x20);
                p.level.spawnz = (ushort) (p.pos[2] / 0x20);
                p.level.rotx = p.rot[0];
                p.level.roty = 0;
            }
        }

        public override string name
        {
            get
            {
                return "setspawn";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Admin;
            }
        }
    }
}

