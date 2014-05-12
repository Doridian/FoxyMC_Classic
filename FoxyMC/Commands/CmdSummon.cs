namespace FoxyMC.Commands
{
    using System;

    public class CmdSummon : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/summon <player> - Summons a player to your position.");
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
                    p.SendMessage("There is no player \"" + player + "\"!");
                }
                else if (p.level != player.level)
                {
                    p.SendMessage(player.name + " is in a different level.");
                }
                else
                {
					if (player == p) { player.SendMessage("Poof! You go nowhere."); return; }
                    player.SendPos(0xff, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0);
                    player.SendMessage("You were summoned by " + p.color + p.name + "&e.");
                }
            }
        }

        public override string name
        {
            get
            {
                return "summon";
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

