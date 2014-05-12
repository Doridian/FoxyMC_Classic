namespace FoxyMC.Commands
{
    using System;

    public class CmdPermissionBuild : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/PerBuild <rank> - Sets build permission for a map.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                int length = message.Split(new char[] { ' ' }).Length;
                if ((length > 2) || (length < 1))
                {
                    this.Help(p);
                }
                else if (length == 1)
                {
                    LevelPermission permission = Level.PermissionFromName(message);
                    if (permission == LevelPermission.Null)
                    {
                        p.SendMessage("Not a valid rank");
                    }
                    else
                    {
                        p.level.permissionbuild = permission;
                        Server.s.Log(p.level.name + " build permission changed to " + message + ".");
                        Player.GlobalMessageLevel(p.level, "build permission changed to " + message + ".");
                    }
                }
                else
                {
                    int index = message.IndexOf(' ');
                    string str = message.Substring(0, index).ToLower();
                    string name = message.Substring(index + 1).ToLower();
                    LevelPermission permission2 = Level.PermissionFromName(name);
                    if (permission2 == LevelPermission.Null)
                    {
                        p.SendMessage("Not a valid rank");
                    }
                    else
                    {
                        foreach (Level level in Server.levels)
                        {
                            if (level.name.ToLower() == str.ToLower())
                            {
                                level.permissionbuild = permission2;
                                Server.s.Log(level.name + " build permission changed to " + name + ".");
                                Player.GlobalMessageLevel(level, "build permission changed to " + name + ".");
                                if (p.level != level)
                                {
                                    p.SendMessage("build permission changed to " + name + " on " + level.name + ".");
                                }
                                return;
                            }
                        }
                        p.SendMessage("There is no level \"" + name + "\" loaded.");
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "perbuild";
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

