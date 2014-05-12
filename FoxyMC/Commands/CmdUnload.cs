namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;

    public class CmdUnload : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/unload [level] - Unloads a level.");
        }

        public override void Use(Player p, string message)
        {
            using (List<Level>.Enumerator enumerator = Server.levels.GetEnumerator())
            {
                Action<Player> action = null;
                Action<PlayerBot> action2 = null;
                Action<Player> action3 = null;
                Action<Player> action4 = null;
                while (enumerator.MoveNext())
                {
                    Level level = enumerator.Current;
                    if (level.name.ToLower() == message.ToLower())
                    {
                        if (level == Server.mainLevel)
                        {
                            p.SendMessage("You can't unload the main level.");
                        }
                        else
                        {
                            if (action == null)
                            {
                                action = delegate (Player pl) {
                                    if (pl.level == level)
                                    {
                                        Player.GlobalDie(pl, true);
                                    }
                                };
                            }
                            Player.players.ForEach(action);
                            if (action2 == null)
                            {
                                action2 = delegate (PlayerBot b) {
                                    if (b.level == level)
                                    {
                                        b.GlobalDie();
                                    }
                                };
                            }
                            PlayerBot.playerbots.ForEach(action2);
                            if (action3 == null)
                            {
                                action3 = delegate (Player pl) {
                                    if (pl.level == level)
                                    {
                                        pl.SendMotd();
                                    }
                                };
                            }
                            Player.players.ForEach(action3);
                            ushort spawnx = Server.mainLevel.spawnx;
                            ushort spawny = Server.mainLevel.spawny;
                            ushort spawnz = Server.mainLevel.spawnz;
                            if (action4 == null)
                            {
                                action4 = delegate (Player pl) {
                                    if (pl.level == level)
                                    {
                                        pl.Kick("Level unloaded, please rejoin.");
                                    }
                                };
                            }
                            Player.players.ForEach(action4);
                            Server.levels.Remove(level);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            if (p.level != level)
                            {
                                p.SendMessage("Level \"" + level.name + "\" unloaded.");
                            }
                            else
                            {
                                p.Kick("Level unloaded, please rejoin.");
                            }
                        }
                        return;
                    }
                }
            }
            p.SendMessage("There is no level \"" + message + "\" loaded.");
        }

        public override string name
        {
            get
            {
                return "unload";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Admin;
            }
        }
    }
}

