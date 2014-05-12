namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class CmdLoad : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/load <level> - Loads a level.");
        }

        public override void Use(Player p, string message)
        {
            Server.ml.Queue(delegate {
                try
                {
                    List<Level> list;
                    if (p == null)
                    {
                        goto Label_0322;
                    }
                    if (message == "")
                    {
                        this.Help(p);
                        return;
                    }
                    if (message.Split(new char[] { ' ' }).Length > 2)
                    {
                        this.Help(p);
                        return;
                    }
                    int index = message.IndexOf(' ');
                    string s = "0";
                    if (index != -1)
                    {
                        s = message.Substring(index + 1);
                        message = message.Substring(0, index).ToLower();
                    }
                    else
                    {
                        message = message.ToLower();
                    }
                    foreach (Level level in Server.levels)
                    {
                        if (level.name == message)
                        {
                            p.SendMessage(message + " is already loaded!");
                            return;
                        }
                    }
                    if (Server.levels.Count == Server.levels.Capacity)
                    {
                        if (Server.levels.Capacity == 1)
                        {
                            p.SendMessage("You can't load any levels!");
                        }
                        else
                        {
                            p.SendMessage("You can't load more than " + Server.levels.Capacity + " levels!");
                        }
                        return;
                    }
                    if (!File.Exists("levels/" + message + ".lvl"))
                    {
                        p.SendMessage("Level \"" + message + "\" doesn't exist!");
                        return;
                    }
                    Level item = Level.Load(message);
                    if (item == null)
                    {
                        if (File.Exists("levels/" + message + ".lvl.backup"))
                        {
                            Server.s.Log("Atempting to load backup.");
                            File.Copy("levels/" + message + ".lvl.backup", "levels/" + message + ".lvl", true);
                            item = Level.Load(message);
                            if (item != null)
                            {
                                goto Label_02A4;
                            }
                            if (!p.disconnected)
                            {
                                p.SendMessage("Backup of " + message + " failed.");
                            }
                        }
                        else if (!p.disconnected)
                        {
                            p.SendMessage("Backup of " + message + " does not exist.");
                        }
                        return;
                    }
                Label_02A4:
                    Monitor.Enter(list = Server.levels);
                    try
                    {
                        Server.levels.Add(item);
                    }
                    finally
                    {
                        Monitor.Exit(list);
                    }
                    Player.GlobalMessage("Level \"" + item.name + "\" loaded.");
                    try
                    {
                        int num2 = int.Parse(s);
                        if ((num2 >= 0) && (num2 <= 2))
                        {
                            item.physics = num2;
                        }
                        goto Label_05C4;
                    }
                    catch
                    {
                        if (!p.disconnected)
                        {
                            p.SendMessage("Physics variable invalid");
                        }
                        goto Label_05C4;
                    }
                Label_0322:;
                    if (message.Split(new char[] { ' ' }).Length > 2)
                    {
                        return;
                    }
                    int length = message.IndexOf(' ');
                    string str2 = "0";
                    if (length != -1)
                    {
                        str2 = message.Substring(length + 1);
                        message = message.Substring(0, length).ToLower();
                    }
                    else
                    {
                        message = message.ToLower();
                    }
                    foreach (Level level3 in Server.levels)
                    {
                        if (level3.name == message)
                        {
                            Server.s.Log(message + " is already loaded!");
                            return;
                        }
                    }
                    if (Server.levels.Count == Server.levels.Capacity)
                    {
                        if (Server.levels.Capacity == 1)
                        {
                            Server.s.Log("You can't load any levels!");
                        }
                        else
                        {
                            Server.s.Log("You can't load more than " + Server.levels.Capacity + " levels!");
                        }
                        return;
                    }
                    if (!File.Exists("levels/" + message + ".lvl"))
                    {
                        Server.s.Log("Level \"" + message + "\" doesn't exist!");
                        return;
                    }
                    Level level4 = Level.Load(message);
                    if (level4 == null)
                    {
                        if (File.Exists("levels/" + message + ".lvl.backup"))
                        {
                            Server.s.Log("Atempting to load backup.");
                            File.Copy("levels/" + message + ".lvl.backup", "levels/" + message + ".lvl", true);
                            level4 = Level.Load(message);
                            if (level4 != null)
                            {
                                goto Label_0568;
                            }
                            Server.s.Log("Backup of " + message + " failed.");
                        }
                        else
                        {
                            Server.s.Log("Backup of " + message + " does not exist.");
                        }
                        return;
                    }
                Label_0568:
                    Server.levels.Add(level4);
                    Server.s.Log("Level \"" + level4.name + "\" loaded.");
                    try
                    {
                        int num4 = int.Parse(str2);
                        if ((num4 >= 0) && (num4 <= 2))
                        {
                            level4.physics = num4;
                        }
                    }
                    catch
                    {
                        Server.s.Log("Physics variable invalid");
                    }
                Label_05C4:
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception exception)
                {
                    Player.GlobalMessage("An error occured with /load");
                    Server.ErrorLog(exception);
                }
            });
        }

        public override string name
        {
            get
            {
                return "load";
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

