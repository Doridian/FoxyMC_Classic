namespace FoxyMC.Commands
{
    using System;
    using System.IO;

    internal class CmdDeleteLvl : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/deletelvl - perminatly deletes a level.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                foreach (Level level in Server.levels)
                {
                    if (level.name.ToLower() == message)
                    {
                        Command.all["unload"].Use(p, message);
                        File.Delete("levels/" + level.name + ".lvl");
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "deletelvl";
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

