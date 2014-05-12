namespace FoxyMC.Commands
{
    using System;

    public class CmdUndoPlayer : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/undoplayer <player> - Undos all changes that player did");
        }

        public override void Use(Player p, string message)
        {
            message = message.Trim().ToLower();
            if (message.Length < 1) return;
            long changes = 0;
            Level l = p.level;
            for (ushort x = 0; x < l.width; x++)
            {
                for (ushort y = 0; y < l.depth; y++)
                {
                    for (ushort z = 0; z < l.height; z++)
                    {
                        if (l.plychanges[x, y, z] == message)
                        {
                            l.Blockchange(x, y, z, l.plyblocks[x, y, z]);
                            changes++;
                        }
                    }
                }
            }
            Player.GlobalMessageLevel(l,changes + " blockchange(s) by " + message + " have been undone!");
        }

        public override string name
        {
            get
            {
                return "undoplayer";
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

