using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FoxyMC.Commands
{
    class CmdFill : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/fill [flat/up/down/all] [type] - Fills the area up with blocks");
        }

        public void Blockchange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            FillStruct fs = (FillStruct)p.blockchangeObject;
            if (fs.block != 0xff) type = fs.block;
            List<Pos> list = new List<Pos>();
            if (!Block.Placable(type))
            {
                p.SendMessage("Sorry, you can't place that with /fill!");
                return;
            }
            if (!this.CheckAdjactentBlocks(list, p, x, y, z, fs.ftype)) return;

            Level l = p.level;
            foreach (Pos po in list)
            {
                l.Blockchange(p, po.x,po.y,po.z, type);
            }
        }

        private bool CheckAdjactentBlocks(List<Pos> list, Player p, ushort x, ushort y, ushort z, FillType ft)
        {
            List<Pos> chked = new List<Pos>();
            Queue<Pos> q = new Queue<Pos>();
            Pos pos;
            pos.x = x; pos.y = y; pos.z = z;
            q.Enqueue(pos);
            while(q.Count > 0) {
                pos = q.Dequeue();
                if (chked.Contains(pos)) continue;
                chked.Add(pos);
                Level l = p.level;
                x = pos.x;
                y = pos.y;
                z = pos.z;
                if (l.GetTile(x, y, z) == Block.air)
                {
                    list.Add(pos);
                }
                else
                {
                    continue;
                }
                if (list.Count > 800)
                {
                    p.SendMessage("Too many blocks!");
                    return false;
                } 
           
                if (x < l.width) 
                {
                    Pos po;
                    po.x = (ushort)(x + 1); po.y = y; po.z = z;
                    q.Enqueue(po); 
                }
                if (x > 0)
                {
                    Pos po;
                    po.x = (ushort)(x - 1); po.y = y; po.z = z;
                    q.Enqueue(po); 
                }
                if (ft != FillType.flat && ft != FillType.down && y < l.depth)
                {
                    Pos po;
                    po.x = x; po.y = (ushort)(y + 1); po.z = z;
                    q.Enqueue(po); 
                }
                if (ft != FillType.flat && ft != FillType.up && y > 0)
                {
                    Pos po;
                    po.x = x; po.y = (ushort)(y - 1); po.z = z;
                    q.Enqueue(po); 
                }
                if (z < l.height)
                {
                    Pos po;
                    po.x = x; po.y = y; po.z = (ushort)(z + 1);
                    q.Enqueue(po); 
                }
                if (z > 0)
                {
                    Pos po;
                    po.x = x; po.y = y; po.z = (ushort)(z - 1);
                    q.Enqueue(po); 
                }
            }

            return true;
        }

        public override void Use(Player p, string message)
        {
            message = message.ToLower().Trim();
            FillType ftype = FillType.all;
            byte btype = 0xff;

            string arg = "";
            int pos = message.IndexOf(' ');
            if (pos > 0)
            {
                arg = message.Substring(pos+1);
                message = message.Substring(0, pos);
            }

            switch (message)
            {
                case "":
                case "all":
                    ftype = FillType.all;
                    break;
                case "up":
                    ftype = FillType.up;
                    break;
                case "down":
                    ftype = FillType.down;
                    break;
                case "flat":
                    ftype = FillType.flat;
                    break;
                default:
                    btype = Block.Byte(message);
                    if (btype == 0xff)
                    {
                        p.SendMessage("Invalid!");
                        return;
                    }
                    break;
            }
            if(arg != null && arg != "")
            {
                btype = Block.Byte(arg);
                if(btype == 0xff)
                {
                    p.SendMessage("Invalid!");
                    return;
                }
            }
            if (btype != 0xff && !Block.Placable(btype))
            {
                p.SendMessage("Sorry, you can't place that with /fill!");
                return;
            }
            FillStruct fs;
            fs.ftype = ftype;
            fs.block = btype;
            p.blockchangeObject = fs;
            p.SendMessage("Break/build a block to start filling from.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange);            
        }

        public override string name
        {
            get { return "fill"; }
        }

        public override LevelPermission level
        {
            get { return LevelPermission.AdvBuilder; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FillStruct
        {
            public FillType ftype;
            public byte block;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Pos
        {
            public ushort x;
            public ushort y;
            public ushort z;
        }

        private enum FillType
        {
            flat,
            up,
            down,
            all
        }
    }
}
