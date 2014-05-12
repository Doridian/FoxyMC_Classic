using System;
using System.Collections.Generic;
using System.Text;

namespace FoxyMC.Gamemodes
{
    class LaveSurvival : TickBasedGamemode
    {
        int ticks;
        byte mode;
        LevelPermission oldperm;

        List<Player> survivors;

        public override void Help(Player p)
        {
            p.SendMessage("This is lava survival. The rules are quite simple");
            p.SendMessage("From the top hole you can see above spawn, lava will begin to flood");
            p.SendMessage("If the lava hits you, you are dead");
            p.SendMessage("Your goal is to survive as long as possible");
            p.SendMessage("You may destroy other player's tunnels to make them flood");
        }

        public override void TimerTick(object t, System.Timers.ElapsedEventArgs e)
        {
            ticks++;
            if (mode == 0)
            {
                if (ticks >= ((int)this.GetOption("pause") * 10))
                {
                    survivors = new List<Player>();
                    foreach (Player p in Player.players)
                    {
                        if(p.level == this.level)
                        {
                            survivors.Add(p);
                        }
                    }
                    if (survivors.Count < 1) { ticks = 0; return; }
                    Player.GlobalMessageLevel(this.level, "The flood has begun!");
                    oldperm = this.level.permissionvisit;
                    if(oldperm < LevelPermission.Operator) this.level.permissionvisit = LevelPermission.Operator;
                    mode = 1;
                    ticks = 0;
                    this.level.physics = 2;
                    this.level.Blockchange((ushort)(int)this.GetOption("startx"), (ushort)(int)this.GetOption("starty"), (ushort)(int)this.GetOption("startz"), Block.lava);
                }
            }
            else if (mode == 1)
            {
                foreach (Player p in survivors)
                {
                    if (p == null
                        || p.name == null
                        || !Player.players.Contains(p)
                        || p.level != this.level
                        || this.level.GetTile((ushort)(p.pos[0] / 0x20), (ushort)(p.pos[1] / 0x20), (ushort)(p.pos[2] / 0x20)) == Block.lava)
                    {
                        if (p != null && p.name != null)
                        {
                            Command.all["goto"].Use(p, "main");
                            Player.GlobalMessageLevel(this.level, p.name + " died!");
                        }
                        survivors.Remove(p);
                    }
                }

                if (ticks >= ((int)this.GetOption("duration") * 10) || survivors.Count <= 1)
                {
                    Player.GlobalMessageLevel(this.level, "The flood has ended!");

                    if (survivors.Count > 0)
                    {
                        string msg = "";
                        foreach (Player p in survivors)
                        {
                            msg += ", " + p.name;
                        }
                        Player.GlobalMessageLevel(this.level, "Survivor(s): " + msg.Remove(0, 2));
                    }
                    else
                    {
                        Player.GlobalMessageLevel(this.level, "Everyone died!");
                    }

                    this.level.permissionvisit = oldperm;
                    mode = 0;
                    this.StopFlood();
                    ticks = 0;
                }
            }
        }

        private void StopFlood()
        {
            Level l = this.level;
            l.physics = 0;
            for (ushort x = 0; x < l.width; x++)
            {
                for (ushort y = 0; y < l.depth; y++)
                {
                    for (ushort z = 0; z < l.height; z++)
                    {
                        if (l.GetTile(x, y, z) == Block.lava)
                        {
                            l.Blockchange(x, y, z, Block.air);
                        }
                    }
                }
            }
        }

        public override void Start()
        {
            base.Start();
            oldperm = this.level.permissionvisit;
            this.Stop();
            mode = 0;
            ticks = 0;
        }
        public override void Stop()
        {
            base.Stop();
            this.level.permissionvisit = oldperm;
            this.StopFlood();
        }

        protected override void InitOptions()
        {
            this.options.Add("pause", 60 * 10);
            this.options.Add("duration", 60 * 5);
            this.options.Add("startx", this.level.width / 2);
            this.options.Add("starty", this.level.depth / 2);
            this.options.Add("startz", this.level.height / 2);
        }

        public override string name
        {
            get { return "lava"; }
        }

        public override bool RunCmd(Player p, string cmd, string args)
        {
            return false;
        }

        public override bool Blockchange(Player p, ushort x, ushort y, ushort z, byte oldtype, byte newtype)
        {
            return true;
        }
    }
}
