namespace FoxyMC.Commands
{
    using System;

    public class CmdPhysics : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/physics <0/1/2> - Set the levels physics, 0-Off 1-On 2-Advanced");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                if (p != null)
                {
                    this.Help(p);
                }
            }
            else
            {
                try
                {
                    int num = int.Parse(message);
                    if ((num >= 0) && (num <= 2))
                    {
                        p.level.physics = num;
                        switch (num)
                        {
                            case 0:
                                p.level.ClearPhysics();
                                Player.GlobalMessageLevel(p.level, "Physics is now &cOFF&e on &b" + p.level.name + "&e.");
                                Server.s.Log("Physics is now OFF on " + p.level.name + ".");
                                return;

                            case 1:
                                Player.GlobalMessageLevel(p.level, "Physics is now &aNormal&e on &b" + p.level.name + "&e.");
                                Server.s.Log("Physics is now ON on " + p.level.name + ".");
                                return;

                            case 2:
                                Player.GlobalMessageLevel(p.level, "Physics is now &aAdvanced&e on &b" + p.level.name + "&e.");
                                Server.s.Log("Physics is now ADVANCED on " + p.level.name + ".");
                                return;
                        }
                    }
                    else if (p != null)
                    {
                        p.SendMessage("Not a valid setting");
                    }
                }
                catch
                {
                    if (p != null)
                    {
                        p.SendMessage("INVALID INPUT");
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "physics";
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

