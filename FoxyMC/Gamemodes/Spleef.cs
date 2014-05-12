using System;
using System.Collections.Generic;
using System.Text;

namespace FoxyMC.Gamemodes
{
    class Spleef : TickBasedGamemode
    {
        bool gamerunning = false;
        List<Player> players = new List<Player>();

        int ticks = 0;

        public override void TimerTick(object t, System.Timers.ElapsedEventArgs e)
        {
            Level l = this.level;
            if (!gamerunning)
            {
                ticks++;
                if (ticks >= ((int)this.GetOption("waittime") * 10))
                {
                    if (players.Count > 1)
                    {
                        gamerunning = true;
                        Player.GlobalMessageLevel(l,"Match has begun. FIGHT! :3");
                        string msg = "";
                        foreach (Player p in players)
                        {
                            msg += ", " + p.name;
                        }
                        Player.GlobalMessageLevel(l, "Players this game: " + msg.Remove(0, 2));
                    }
                    else
                    {
                        Player.GlobalMessageLevel(l,"Sorry, not enough players (Please rejoin!)");
                        this.StartGame();
                    }
                }
                return;
            }
            ushort y = (ushort)(int)this.GetOption("zlevel");
            foreach (Player p in players)
            {
                if (p == null
                    || p.name == null
                    || !Player.players.Contains(p)
                    || p.level != this.level
                    || (p.pos[1] / 0x20) < y
                    )
                {
                    if (p != null && p.name != null)
                    {
                        Player.GlobalMessageLevel(l, p.name + " died!");
                        if (p.level == this.level)
                        {
                            Command.all["spawn"].Use(p, "");
                        }
                    }
                    players.Remove(p);
                }
            }
            if (players.Count == 1)
            {
                Player p = players[0];
                Player.GlobalMessageLevel(l, p.name + " won! Gratz!");
                Command.all["spawn"].Use(p, "");
                this.StartGame();
            }
            else if (players.Count < 1)
            {
                Player.GlobalMessageLevel(l, "Nobody won O.o");
                this.StartGame();
            }
        }

        private void StartGame()
        {
            Level l = this.level;
            ushort y = (ushort)(int)this.GetOption("zlevel");
            for (ushort x = 1; x < l.width - 1; x++)
            {
                for (ushort z = 1; z < l.height - 1; z++)
                {
                    l.Blockchange(x, y, z, Block.trunk);
                }
            }
            players = new List<Player>();
            Player.GlobalMessageLevel(l,"Time for a new game!");
            Player.GlobalMessageLevel(l,"To join, type /gm join");
            gamerunning = false;
            ticks = 0;
        }

        public override void Help(Player p)
        {
            
        }

        public override void Start()
        {
            base.Start();
            this.StartGame();
            gamerunning = true;
        }

        protected override void InitOptions()
        {
            this.options.Add("zlevel", this.level.height / 2);
            this.options.Add("waittime", 120);
        }

        public override string name
        {
            get { return "spleef"; }
        }

        public override bool RunCmd(Player p, string cmd, string args)
        {
            if (cmd == "join")
            {
                if (gamerunning)
                {
                    p.SendMessage("Sorry, game is currently running!");
                }
                else if (players.Contains(p))
                {
                    p.SendMessage("You are already in!");
                }
                else
                {
                    Player.GlobalMessageLevel(this.level, p.name + " joined!");
                    players.Add(p);
                }
                
                return true;
            }
            return false;
        }

        public override bool Blockchange(Player p, ushort x, ushort y, ushort z, byte oldtype, byte newtype)
        {
            if (!gamerunning && p.group.Permission >= LevelPermission.Operator) return true;
            if (!gamerunning || oldtype != Block.trunk || newtype != Block.air || y != (ushort)(int)this.GetOption("zlevel") || !players.Contains(p)) return false;
            return true;
        }
    }
}
