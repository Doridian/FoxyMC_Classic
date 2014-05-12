namespace FoxyMC.Commands
{
    using System;

    public class CmdOldRanks : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("This command is Deprecated. Please use the following:");
            Command.all["setrank"].Help(p);
        }

        public override void Use(Player p, string message)
        {
            p.SendMessage("This command is Deprecated. Please use the following:");
            Command.all["setrank"].Help(p);
        }

        public override string name
        {
            get
            {
                return "oldranks";
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

