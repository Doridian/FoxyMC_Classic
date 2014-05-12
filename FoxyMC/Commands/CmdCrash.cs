namespace FoxyMC.Commands
{
    using System;

    public class CmdCrash : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/crash <name> - Crashs a player.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { this.Help(p); return; }
            if (!Player.Exists(message)) { p.SendMessage("No one with that name found!"); return; }
            Player targ = Player.Find(message);
            if (targ.group.Permission > p.group.Permission) { p.SendMessage("Acess denied!"); return; }
            targ.SendMessage("\0");
            p.SendMessage("Crashed " + targ.name + "!");
        }

        public override string name
        {
            get
            {
                return "crash";
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

