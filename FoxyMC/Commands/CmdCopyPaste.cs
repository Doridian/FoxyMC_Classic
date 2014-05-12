namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class CmdCopy : Command
    {
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte num = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, num);
            CatchPosX blockchangeObject = (CatchPosX) p.blockchangeObject;
            blockchangeObject.x = x;
            blockchangeObject.y = y;
            blockchangeObject.z = z;
            p.blockchangeObject = blockchangeObject;
            p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange2);
        }

        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte num = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, num);
            CatchPosX blockchangeObject = (CatchPosX) p.blockchangeObject;

            List<BlockX> list = new List<BlockX>();

            ushort xdiff = (ushort)Math.Abs((int)(blockchangeObject.x - x));
            ushort ydiff = (ushort)Math.Abs((int)(blockchangeObject.y - y));
            ushort zdiff = (ushort)Math.Abs((int)(blockchangeObject.z - z));

            int size = Math.Abs(xdiff * ydiff * zdiff);

            if (!p.CheckBlocklimit(list.Count))
            {
                p.SendMessage("Too many blocks!");
                return;
            }

            list.Capacity = size;
            for (ushort i = Math.Min(blockchangeObject.x, x); i <= Math.Max(blockchangeObject.x, x); i = (ushort)(i + 1))
            {
                for (ushort j = Math.Min(blockchangeObject.y, y); j <= Math.Max(blockchangeObject.y, y); j = (ushort)(j + 1))
                {
                    for (ushort k = Math.Min(blockchangeObject.z, z); k <= Math.Max(blockchangeObject.z, z); k = (ushort)(k + 1))
                    {
                        this.BufferAdd(list, (short)(i - blockchangeObject.x), (short)(j - blockchangeObject.y), (short)(k - blockchangeObject.z), p.level.GetTile(i, j, k));
                    }
                }
            }

            p.SendMessage(list.Count.ToString() + " blocks.");
            if (p.group.Permission <= LevelPermission.AdvBuilder)
            {
                foreach(BlockX pos in list) {
                    byte num1 = p.level.GetTile((ushort)pos.x, (ushort)pos.y, (ushort)pos.z);
                    if (!(Block.Placable(num1) || Block.AdvPlacable(num1)))
                    {
                        p.SendMessage("Sorry, contains blocks you may not place.");
                        p.blockchangeObject = null;
                        return;
                    }
                }
            }

            blockchangeObject.list = list;
            blockchangeObject.x = xdiff;
            blockchangeObject.y = ydiff;
            blockchangeObject.z = zdiff;
            p.blockchangeObject = blockchangeObject;

            //p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange3);
            p.SendMessage("Buffer saved! Use /paste now :3");

        }

        private void BufferAdd(List<BlockX> list, short x, short y, short z, byte t)
        {
            BlockX b;
            b.x = x;
            b.y = y;
            b.z = z;
            b.t = t;
            list.Add(b);
        }

        public override void Help(Player p)
        {
            p.SendMessage("/copy - copies \"some\" blocks.");
        }

        public override void Use(Player p, string message)
        {
            p.SendMessage("Place/break two blocks to determine the edges.");
            p.ClearBlockchange();
            p.blockchangeObject = new CatchPosX();
            p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange1);
        }

        public override string name
        {
            get
            {
                return "copy";
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

    public class CmdPaste : Command
    {
        public void Blockchange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte num = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, num);

            CatchPosX blockchangeObject = (CatchPosX)p.blockchangeObject;

            foreach (BlockX b in blockchangeObject.list)
            {
                p.level.Blockchange(p, (ushort)(x + b.x), (ushort)(y + b.y), (ushort)(z + b.z), b.t);
            }
        }

        
        public override void Help(Player p)
        {
            p.SendMessage("/paste - pastes previously copied blocks.");
        }

        public override void Use(Player p, string message)
        {
            if (p.blockchangeObject == null || p.blockchangeObject.GetType() != typeof(CatchPosX))
            {
                p.SendMessage("Sorry, you don't have any copied blocks!");
                return;
            }
            p.SendMessage("Place/break a block where to paste.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange);
        }

        public override string name
        {
            get
            {
                return "paste";
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


    [StructLayout(LayoutKind.Sequential)]
    internal struct CatchPosX
    {
        public ushort x;
        public ushort y;
        public ushort z;
        public List<BlockX> list;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockX
    {
        public short x;
        public short y;
        public short z;
        public byte t;
    }
}

