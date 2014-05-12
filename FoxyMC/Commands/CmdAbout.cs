namespace FoxyMC.Commands
{
    using System;

    public class CmdAbout : Command
    {
        public void Blockchange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte num = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, num);
            if (num == 0xff)
            {
                p.SendMessage(string.Concat(new object[] { "Invalid Block(", x, ",", y, ",", z, ")!" }));
            }
            else
            {
                object obj2 = string.Concat(new object[] { "Block (", x, ",", y, ",", z, "): " });
                p.SendMessage(string.Concat(new object[] { obj2, "&f", num, " = ", Block.Name(num) }) + "&e.");
                string owner = p.level.plychanges[x, y, z];
                if (owner == null || owner == "")
                {
                    p.SendMessage("Not changed since map load");
                }
                else
                {
                    p.SendMessage("Last changed by: &f" + owner + "&e.");
                }
            }
        }

        public override void Help(Player p)
        {
            p.SendMessage("/about - Displays information about a block.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                p.SendMessage("Break/build a block to display information.");
                p.ClearBlockchange();
                p.Blockchange += new Player.BlockchangeEventHandler(this.Blockchange);
            }
        }

        public override string name
        {
            get
            {
                return "about";
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

