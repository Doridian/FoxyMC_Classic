namespace FoxyMC.Commands
{
    using System;

    public class CmdHide : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/hide - Makes yourself (in)visible to other players.");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                this.Help(p);
            }
            else
            {
                p.hidden = !p.hidden;
                if (p.hidden)
                {
                    Player.GlobalDie(p, true);
                    Player.GlobalMessageOps("To Ops &f-" + p.color + p.name + "&f-&e is now &finvisible&e.");
                    Player.GlobalChat(p, "&c- " + p.color + p.name + "&e disconnected.", false);
                }
                else
                {
                    Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                    Player.GlobalMessageOps("To Ops &f-" + p.color + p.name + "&f-&e is now &8visible&e.");
                    Player.GlobalChat(p, "&a+ " + p.color + p.name + "&e joined the game.", false);
                }
            }
        }

        public override string name
        {
            get
            {
                return "hide";
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

