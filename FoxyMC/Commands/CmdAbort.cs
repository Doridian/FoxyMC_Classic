namespace FoxyMC.Commands
{
    using System;

    public class CmdAbort : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/abort - Cancels an action.");
        }

        public override void Use(Player p, string message)
        {
            if (p.HasBlockchange())
            {
                p.SendMessage("There is no action to abort.");
            }
            else
            {
                p.ClearBlockchange();
                p.SendMessage("Action aborted.");
            }
        }

        public override string name
        {
            get
            {
                return "abort";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.AdvBuilder;
            }
        }
    }
}

