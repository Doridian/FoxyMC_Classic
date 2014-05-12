namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class CmdLevels : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/levels - Lists all levels.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                List<string> levels = new List<string>(Server.levels.Count);
                message = Server.mainLevel.name;
                string message2 = "";
                levels.Add(Server.mainLevel.name.ToLower());
                bool Once = false;
                foreach(Level level in Server.levels)
                {
                    if (level != Server.mainLevel)
                    {
                        if (level.permissionvisit <= p.group.Permission)
                        {
                            message = message + ", " + level.name;
                            levels.Add(level.name.ToLower());
                        }
                        else if (!Once)
                        {
                            Once = true;
                            message2 = message2 + level.name;
                        }
                        else
                        {
                            message2 = message2 + ", " + level.name;
                        }
                    }
                }
                p.SendMessage("Loaded: &2" + message);
                p.SendMessage("Can't Goto: &c" + message2);
                message = "";
                FileInfo[] files = new DirectoryInfo("levels/").GetFiles("*.lvl");
                Once = false;
                foreach (FileInfo info2 in files)
                {
                    if (!levels.Contains(info2.Name.Replace(".lvl", "").ToLower()))
                    {
                        if (!Once)
                        {
                            Once = true;
                            message = message + info2.Name.Replace(".lvl", "");
                        }
                        else
                        {
                            message = message + ", " + info2.Name.Replace(".lvl", "");
                        }
                    }
                }
                p.SendMessage("Unloaded: &4" + message);
            }
        }

        public override string name
        {
            get
            {
                return "levels";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return (Server.guestGoto) ? LevelPermission.Guest : LevelPermission.Builder;
            }
        }
    }
}

