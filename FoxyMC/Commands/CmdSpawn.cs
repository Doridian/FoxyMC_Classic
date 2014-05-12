namespace FoxyMC.Commands
{
    using System;

    public class CmdSpawn : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/spawn - Teleports yourself to the spawn location.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                ushort x = (ushort) ((0.5 + p.level.spawnx) * 32.0);
                ushort y = (ushort) ((1 + p.level.spawny) * 0x20);
                ushort z = (ushort) ((0.5 + p.level.spawnz) * 32.0);
                p.SendPos(0xff, x, y, z, p.level.rotx, p.level.roty);
            }
        }

        public override string name
        {
            get
            {
                return "spawn";
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

