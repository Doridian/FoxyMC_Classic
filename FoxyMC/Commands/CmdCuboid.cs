namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class CmdCuboid : Command
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
            switch (blockchangeObject.solid)
            {
                case SolidType.solid:
                    list.Capacity = (Math.Abs((int) (blockchangeObject.x - x)) * Math.Abs((int) (blockchangeObject.y - y))) * Math.Abs((int) (blockchangeObject.z - z));
                    for (ushort i = Math.Min(blockchangeObject.x, x); i <= Math.Max(blockchangeObject.x, x); i = (ushort) (i + 1))
                    {
                        for (ushort j = Math.Min(blockchangeObject.y, y); j <= Math.Max(blockchangeObject.y, y); j = (ushort) (j + 1))
                        {
                            for (ushort k = Math.Min(blockchangeObject.z, z); k <= Math.Max(blockchangeObject.z, z); k = (ushort) (k + 1))
                            {
                                if (p.level.GetTile(i, j, k) != type)
                                {
                                    this.BufferAdd(list, i, j, k);
                                }
                            }
                        }
                    }
                    break;

                case SolidType.hollow:
                    for (ushort m = Math.Min(blockchangeObject.y, y); m <= Math.Max(blockchangeObject.y, y); m = (ushort) (m + 1))
                    {
                        for (ushort n = Math.Min(blockchangeObject.z, z); n <= Math.Max(blockchangeObject.z, z); n = (ushort) (n + 1))
                        {
                            if (p.level.GetTile(blockchangeObject.x, m, n) != type)
                            {
                                this.BufferAdd(list, blockchangeObject.x, m, n);
                            }
                            if ((blockchangeObject.x != x) && (p.level.GetTile(x, m, n) != type))
                            {
                                this.BufferAdd(list, x, m, n);
                            }
                        }
                    }
                    if (Math.Abs((int) (blockchangeObject.x - x)) >= 2)
                    {
                        for (ushort num7 = (ushort) (Math.Min(blockchangeObject.x, x) + 1); num7 <= (Math.Max(blockchangeObject.x, x) - 1); num7 = (ushort) (num7 + 1))
                        {
                            for (ushort num8 = Math.Min(blockchangeObject.z, z); num8 <= Math.Max(blockchangeObject.z, z); num8 = (ushort) (num8 + 1))
                            {
                                if (p.level.GetTile(num7, blockchangeObject.y, num8) != type)
                                {
                                    this.BufferAdd(list, num7, blockchangeObject.y, num8);
                                }
                                if ((blockchangeObject.y != y) && (p.level.GetTile(num7, y, num8) != type))
                                {
                                    this.BufferAdd(list, num7, y, num8);
                                }
                            }
                        }
                        if (Math.Abs((int) (blockchangeObject.y - y)) >= 2)
                        {
                            for (ushort num9 = (ushort) (Math.Min(blockchangeObject.x, x) + 1); num9 <= (Math.Max(blockchangeObject.x, x) - 1); num9 = (ushort) (num9 + 1))
                            {
                                for (ushort num10 = (ushort) (Math.Min(blockchangeObject.y, y) + 1); num10 <= (Math.Max(blockchangeObject.y, y) - 1); num10 = (ushort) (num10 + 1))
                                {
                                    if (p.level.GetTile(num9, num10, blockchangeObject.z) != type)
                                    {
                                        this.BufferAdd(list, num9, num10, blockchangeObject.z);
                                    }
                                    if ((blockchangeObject.z != z) && (p.level.GetTile(num9, num10, z) != type))
                                    {
                                        this.BufferAdd(list, num9, num10, z);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case SolidType.walls:
                    for (ushort num11 = Math.Min(blockchangeObject.y, y); num11 <= Math.Max(blockchangeObject.y, y); num11 = (ushort) (num11 + 1))
                    {
                        for (ushort num12 = Math.Min(blockchangeObject.z, z); num12 <= Math.Max(blockchangeObject.z, z); num12 = (ushort) (num12 + 1))
                        {
                            if (p.level.GetTile(blockchangeObject.x, num11, num12) != type)
                            {
                                this.BufferAdd(list, blockchangeObject.x, num11, num12);
                            }
                            if ((blockchangeObject.x != x) && (p.level.GetTile(x, num11, num12) != type))
                            {
                                this.BufferAdd(list, x, num11, num12);
                            }
                        }
                    }
                    if ((Math.Abs((int) (blockchangeObject.x - x)) >= 2) && (Math.Abs((int) (blockchangeObject.z - z)) >= 2))
                    {
                        for (ushort num13 = (ushort) (Math.Min(blockchangeObject.x, x) + 1); num13 <= (Math.Max(blockchangeObject.x, x) - 1); num13 = (ushort) (num13 + 1))
                        {
                            for (ushort num14 = Math.Min(blockchangeObject.y, y); num14 <= Math.Max(blockchangeObject.y, y); num14 = (ushort) (num14 + 1))
                            {
                                if (p.level.GetTile(num13, num14, blockchangeObject.z) != type)
                                {
                                    this.BufferAdd(list, num13, num14, blockchangeObject.z);
                                }
                                if ((blockchangeObject.z != z) && (p.level.GetTile(num13, num14, z) != type))
                                {
                                    this.BufferAdd(list, num13, num14, z);
                                }
                            }
                        }
                    }
                    break;
            }
            if (!p.CheckBlocklimit(list.Count))
            {
                p.SendMessage("Too many blocks!");
                return;
            }
            p.SendMessage(list.Count.ToString() + " blocks.");
            if (p.group.Permission >= LevelPermission.Operator)
            {
                foreach (Pos pos in list)
                {
                    p.level.Blockchange(p, pos.x, pos.y, pos.z, type);
                }
            }
            else
            {
                foreach(Pos pos in list)
                {
                    byte num1 = p.level.GetTile(pos.x, pos.y, pos.z);
                    if (Block.Placable(num1) || Block.AdvPlacable(num1))
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
            p.SendMessage("/cuboid [type] <solid/hollow/walls> - create a cuboid of blocks.");
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
                    if ((p.group.Permission <= LevelPermission.AdvBuilder) && !Block.Placable(num3) && !Block.AdvPlacable(num3))
                    {
                        p.SendMessage("Your not allowed to place that.");
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
                    else if (str2 == "walls")
                    {
                        solid = SolidType.walls;
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
                    else if (message == "walls")
                    {
                        hollow = SolidType.walls;
                    }
                    else
                    {
                        byte type = Block.Byte(message);
                        if (type == 0xff)
                        {
                            p.SendMessage("There is no block \"" + message + "\".");
                            return;
                        }
                        if ((p.group.Permission <= LevelPermission.AdvBuilder) && !Block.Placable(type) && !Block.AdvPlacable(type))
                        {
                            p.SendMessage("Your not allowed to place that.");
                            return;
                        }
                        num4 = type;
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
                return "cuboid";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.AdvBuilder;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CatchPos
        {
            public CmdCuboid.SolidType solid;
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
            hollow,
            walls
        }
    }
}

