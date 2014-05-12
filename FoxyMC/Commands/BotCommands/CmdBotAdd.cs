namespace FoxyMC.Commands
{
    using System;

    public class CmdBotAdd : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/botadd <name> - Add a  new bot at your position.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                PlayerBot bot = PlayerBot.Find(message);
                if (bot != null)
                {
                    p.SendMessage("bot " + bot.name + " already exists!");
                }
                else if (!PlayerBot.ValidName(message))
                {
                    p.SendMessage("bot name " + message + " not valid!");
                }
                else
                {
                    PlayerBot.playerbots.Add(new PlayerBot(message, p.level, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0));
                }
            }
        }

        public override string name
        {
            get
            {
                return "botadd";
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

