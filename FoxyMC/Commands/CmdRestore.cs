namespace FoxyMC.Commands
{
    using System;
    using System.IO;

    internal class CmdRestore : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/restore <number> - restores a previous backup of the current map");
        }

        public override void Use(Player p, string message)
        {
            Action<Player> action = null;
            if (message != "")
            {
                Server.s.Log("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl");
                if (!File.Exists("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl"))
                {
                    p.SendMessage("Backup " + message + " does not exist.");
                }
                else
                {
                    try
                    {
                        File.Copy("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl", "levels/" + p.level.name + ".lvl", true);
                        Level level = Level.Load(p.level.name);
                        if (level != null)
                        {
                            p.level.spawnx = level.spawnx;
                            p.level.spawny = level.spawny;
                            p.level.spawnz = level.spawnz;
                            p.level.height = level.height;
                            p.level.width = level.width;
                            p.level.depth = level.depth;
                            p.level.blocks = level.blocks;
                            p.level.physics = 0;
                            p.level.ClearPhysics();
                            if (action == null)
                            {
                                action = delegate (Player pl) {
                                    if ((pl != p) && (pl.level == p.level))
                                    {
                                        pl.Kick("Restore in progress, please rejoin.");
                                    }
                                };
                            }
                            Player.players.ForEach(action);
                            p.SendMotd();
                            p.SendMap();
                            ushort x = (ushort) ((0.5 + p.level.spawnx) * 32.0);
                            ushort y = (ushort) ((1 + p.level.spawny) * 0x20);
                            ushort z = (ushort) ((0.5 + p.level.spawnz) * 32.0);
                            p.SendPos(0xff, x, y, z, p.level.rotx, p.level.roty);
                        }
                        else
                        {
                            Server.s.Log("Restore nulled");
                            File.Copy("levels/" + p.level.name + ".lvl.backup", "levels/" + p.level.name + ".lvl", true);
                        }
                    }
                    catch
                    {
                        Server.s.Log("Restore fail");
                    }
                }
            }
            else if (Directory.Exists("levels/backups/" + p.level.name))
            {
                p.SendMessage(p.level.name + " has " + Directory.GetDirectories("levels/backups/" + p.level.name).Length.ToString() + " backups .");
            }
            else
            {
                p.SendMessage(p.level.name + " has no backups yet.");
            }
        }

        public override string name
        {
            get
            {
                return "restore";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Operator;
            }
        }
    }
}

