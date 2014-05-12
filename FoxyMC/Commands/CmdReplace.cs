namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class CmdReplace : Command
    {
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte num = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, num);
            CatchPos blockchangeObject = (CatchPos) p.blockchangeObject;
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
            CatchPos cpos = (CatchPos) p.blockchangeObject;
            if (cpos.type != 0xff)
            {
                type = cpos.type;
            }
            List<Pos> list = new List<Pos>();
            for (ushort i = Math.Min(cpos.x, x); i <= Math.Max(cpos.x, x); i = (ushort) (i + 1))
            {
                for (ushort j = Math.Min(cpos.y, y); j <= Math.Max(cpos.y, y); j = (ushort) (j + 1))
                {
                    for (ushort k = Math.Min(cpos.z, z); k <= Math.Max(cpos.z, z); k = (ushort) (k + 1))
                    {
                        if (p.level.GetTile(i, j, k) == type)
                        {
                            this.BufferAdd(list, i, j, k);
                        }
                    }
                }
            }
            if (!p.CheckBlocklimit(list.Count))
            {
                p.SendMessage("Too many blocks!");
                return;
            }
            p.SendMessage(list.Count.ToString() + " blocks.");
            list.ForEach(delegate (Pos pos) {
                p.level.Blockchange(p, pos.x, pos.y, pos.z, cpos.type2);
            });
        }

        private void BufferAdd(List<Pos> list, ushort x, ushort y, ushort z)
        {
            Pos pos;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            list.Add(pos);
        }

        public override void Help(Player p)
        {
            p.SendMessage("/replace [type] [type2] - replace type with type2 inside a selected cuboid");
        }

        public override void Use(Player p, string message)
        {
            if (message.Split(new char[] { ' ' }).Length != 2)
            {
                this.Help(p);
            }
            else
            {
                int index = message.IndexOf(' ');
                string type = message.Substring(0, index).ToLower();
                string str2 = message.Substring(index + 1).ToLower();
                byte num3 = Block.Byte(type);
                if (num3 == 0xff)
                {
                    p.SendMessage("There is no block \"" + type + "\".");
                }
                else
                {
                    byte num4 = Block.Byte(str2);
                    if (num4 == 0xff)
                    {
                        p.SendMessage("There is no block \"" + str2 + "\".");
                    }
                    else
                    {
                        CatchPos pos;
                        pos.type2 = num4;
                        pos.type = num3;
                        pos.x = 0;
                        pos.y = 0;
                        pos.z = 0;
                        p.blockchangeObject = pos;
                        p.SendMessage("Place two blocks to determine the edges.");
                        p.ClearBlockchange();
                        p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange1);
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "replace";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Operator;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CatchPos
        {
            public byte type;
            public byte type2;
            public ushort x;
            public ushort y;
            public ushort z;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Pos
        {
            public ushort x;
            public ushort y;
            public ushort z;
        }
    }
}

