namespace FoxyMC.Commands
{
    using System;

    public class CmdSave : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/save - Saves the level, not an actual backup.");
        }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                if (message != "")
                {
                    this.Help(p);
                }
                else
                {
                    p.level.Save();
                    p.SendMessage("Level \"" + p.level.name + "\" saved.");
                }
            }
            else
            {
                foreach (Level level in Server.levels)
                {
                    level.Save();
                }
            }
        }

        public override string name
        {
            get
            {
                return "save";
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

