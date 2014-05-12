namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class CmdCircle : Command
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
            CatchPos blockchangeObject = (CatchPos) p.blockchangeObject;
            if (blockchangeObject.type != 0xff)
            {
                type = blockchangeObject.type;
            }
            List<Pos> list = new List<Pos>();
            if (Math.Abs((int) (x - blockchangeObject.x)) != Math.Abs((int) (z - blockchangeObject.z)))
            {
                p.SendMessage("No good, make it a circle.");
            }
            else
            {
                float num2 = this.Middle(x, blockchangeObject.x);
                float num3 = this.Middle(z, blockchangeObject.z);
                float num4 = Math.Abs((float) (num2 - x));
                if (num4 != ((int) num4))
                {
                    p.SendMessage("No good, try a diferent radius.");
                }
                else
                {
                    switch (blockchangeObject.solid)
                    {
                        case SolidType.solid:
                        {
                            float num5 = -num4;
                            float num6 = num4;
                            float num7 = 0f;
                            p.SendMessage("Radius " + num4.ToString());
                            while (num6 >= num7)
                            {
                                for (ushort i = Math.Min(blockchangeObject.y, y); i <= Math.Max(blockchangeObject.y, y); i = (ushort) (i + 1))
                                {
                                    this.BufferAdd(list, (ushort) (num2 + num6), i, (ushort) (num3 + num7));
                                    this.BufferAdd(list, (ushort) (num2 - num6), i, (ushort) (num3 + num7));
                                    this.BufferAdd(list, (ushort) (num2 + num6), i, (ushort) (num3 - num7));
                                    this.BufferAdd(list, (ushort) (num2 - num6), i, (ushort) (num3 - num7));
                                    this.BufferAdd(list, (ushort) (num2 + num7), i, (ushort) (num3 + num6));
                                    this.BufferAdd(list, (ushort) (num2 - num7), i, (ushort) (num3 + num6));
                                    this.BufferAdd(list, (ushort) (num2 + num7), i, (ushort) (num3 - num6));
                                    this.BufferAdd(list, (ushort) (num2 - num7), i, (ushort) (num3 - num6));
                                }
                                num5 += num7;
                                num7++;
                                num5 += num7;
                                if (num5 >= 0f)
                                {
                                    num6--;
                                    num5 -= num6;
                                    num5 -= num6;
                                }
                            }
                            break;
                        }
                        case SolidType.hollow:
                            p.SendMessage("Not implemented yet.");
                            return;
                    }
                    if (!p.CheckBlocklimit(list.Count))
                    {
                        p.SendMessage("Too many blocks!");
                        return;
                    }
                    p.SendMessage(list.Count.ToString() + " blocks.");
                    foreach(Pos pos in list)
                    {
                        p.level.Blockchange(p, pos.x, pos.y, pos.z, type);
                    }
                }
            }
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
            p.SendMessage("/circle [type] [solid/hollow] - Its not that good.");
        }

        private float Middle(ushort A, ushort B)
        {
            return (B + ((A - B) / 2f));
        }

        public override void Use(Player p, string message)
        {
            int length = message.Split(new char[] { ' ' }).Length;
            if (length > 2)
            {
                this.Help(p);
            }
            else
            {
                if (length == 2)
                {
                    SolidType solid;
                    CatchPos pos;
                    int index = message.IndexOf(' ');
                    string str = message.Substring(0, index).ToLower();
                    string str2 = message.Substring(index + 1).ToLower();
                    byte num3 = Block.Byte(str);
                    if (num3 == 0xff)
                    {
                        p.SendMessage("There is no block \"" + str + "\".");
                        return;
                    }
                    if (str2 == "solid")
                    {
                        solid = SolidType.solid;
                    }
                    else if (str2 == "hollow")
                    {
                        solid = SolidType.hollow;
                    }
                    else
                    {
                        this.Help(p);
                        return;
                    }
                    pos.solid = solid;
                    pos.type = num3;
                    pos.x = 0;
                    pos.y = 0;
                    pos.z = 0;
                    p.blockchangeObject = pos;
                }
                else if (message != "")
                {
                    CatchPos pos2;
                    SolidType hollow = SolidType.solid;
                    message = message.ToLower();
                    byte num4 = 0xff;
                    if (message == "solid")
                    {
                        hollow = SolidType.solid;
                    }
                    else if (message == "hollow")
                    {
                        hollow = SolidType.hollow;
                    }
                    else
                    {
                        byte num5 = Block.Byte(message);
                        if (num5 == 0xff)
                        {
                            p.SendMessage("There is no block \"" + num5 + "\".");
                            return;
                        }
                        num4 = num5;
                    }
                    pos2.solid = hollow;
                    pos2.type = num4;
                    pos2.x = 0;
                    pos2.y = 0;
                    pos2.z = 0;
                    p.blockchangeObject = pos2;
                }
                else
                {
                    CatchPos pos3;
                    pos3.solid = SolidType.solid;
                    pos3.type = 0xff;
                    pos3.x = 0;
                    pos3.y = 0;
                    pos3.z = 0;
                    p.blockchangeObject = pos3;
                }
                p.SendMessage("Place two blocks to determine the edges.");
                p.ClearBlockchange();
                p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange1);
            }
        }

        public override string name
        {
            get
            {
                return "circle";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Admin;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CatchPos
        {
            public CmdCircle.SolidType solid;
            public byte type;
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

        private enum SolidType
        {
            solid,
            hollow
        }
    }

}

