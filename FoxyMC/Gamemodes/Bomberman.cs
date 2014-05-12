using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FoxyMC.Gamemodes
{
    class Bomberman : TickBasedGamemode
    {
        private List<Pos> tntblocks = new List<Pos>();
        private List<Pos> lavablocks = new List<Pos>();
        private List<Pos> tmp;

        int curtick = 0;
        int bwidth = 5;

        public override void TimerTick(object t, System.Timers.ElapsedEventArgs e)
        {
            Level l = this.level;
            ushort y = (ushort)(int)this.GetOption("zlevel");
            ushort y2 = (ushort)(y - 1);
            foreach (Player p in Player.players)
            {
                if(p.level == this.level && (p.pos[1] / 0x20) <= y)
                {
                    Player.GlobalMessageLevel(l, p.name + " died!");
                    Command.all["spawn"].Use(p, "");
                }
            }
            tmp = new List<Pos>();
            foreach (Pos pos in lavablocks)
            {
                if (pos.targettick <= curtick)
                {
                    l.Blockchange(pos.x, y2, pos.z,Block.stone);
                    tmp.Add(pos);
                }
            }
            foreach (Pos pos in tmp)
            {
                lavablocks.Remove(pos);
            }
            tmp = new List<Pos>();
            
            foreach (Pos pos in tntblocks)
            {
                if(pos.targettick <= curtick)
                {

                    __DetonateTnt(pos, y, y2, l);
                }
            }
            foreach (Pos pos in tmp)
            {
                tntblocks.Remove(pos);
            }
            tmp = null;
            curtick++;
        }

        private void __DetonateTnt(Pos pos, ushort y, ushort y2, Level l)
        {
            l.Blockchange(pos.x, y, pos.z, Block.air);
            for (int x = -bwidth; x <= bwidth; x++)
            {
                Pos px;
                px.targettick = curtick + 10;

                px.x = (ushort)(x + pos.x);
                px.z = pos.z;

                l.Blockchange(px.x, y2, px.z, Block.lava);
                lavablocks.Add(px);

                if (l.GetTile(px.x, y, px.z) == Block.tnt) __DetonateTnt(px, y, y2, l);

                px.x = pos.x;
                px.z = (ushort)(x + pos.z);

                l.Blockchange(px.x, y2, px.z, Block.lava);
                lavablocks.Add(px);

                if (l.GetTile(px.x, y, px.z) == Block.tnt) __DetonateTnt(px, y, y2, l);

                tmp.Add(pos);
            }
        }

        public override bool Blockchange(Player p, ushort x, ushort y, ushort z, byte oldtype, byte newtype)
        {
            ushort ylevel = (ushort)(int)this.GetOption("zlevel");
            if (y == ylevel && oldtype != Block.stone && ((newtype == Block.tnt && oldtype == Block.air) || (oldtype != Block.tnt && newtype == Block.air)))
            {
                if (newtype == Block.tnt)
                {
                    Pos px;
                    px.x = x;
                    px.z = z;
                    px.targettick = curtick + 20;
                    tntblocks.Add(px);
                }
                return true;
            }
            return false;
        }

        public override void Help(Player p)
        {
            
        }

        public override bool RunCmd(Player p, string cmd, string args)
        {
            return false;
        }

        public override void Start()
        {
            curtick = 0;
            base.Start();
        }

        protected override void InitOptions()
        {
            this.options.Add("zlevel", this.level.height / 2);
        }

        public override string name
        {
            get { return "bomberman"; }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Pos
        {
            public ushort x;
            //public ushort y;
            public ushort z;
            public int targettick;
        }
    }
}
