namespace FoxyMC.Commands
{
    using System;

    public class CmdTp : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/tp <player> - Teleports yourself to a player.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                Player player = Player.Find(message);
                if (player == null)
                {
                    p.SendMessage("There is no player \"" + message + "\"!");
                }
                else if (p.level != player.level)
                {
                    p.SendMessage("-" + player.group.color + player.name + "&e- is on &b" + player.level.name + "&e.");
                }
                else
                {
					if (player == p) { player.SendMessage("Your already at yourself."); return; }
                    p.SendPos(0xff, player.pos[0], player.pos[1], player.pos[2], player.rot[0], 0);
                }
            }
        }

        public override string name
        {
            get
            {
                return "tp";
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

