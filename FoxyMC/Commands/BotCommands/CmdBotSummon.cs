namespace FoxyMC.Commands
{
    using System;

    public class CmdBotSummon : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/botsummon <name> - Summons a bot to your position.");
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
                if (bot == null)
                {
                    p.SendMessage("There is no bot " + message + "!");
                }
                else if (p.level != bot.level)
                {
                    p.SendMessage(bot.name + " is in a different level.");
                }
                else
                {
                    bot.SetPos(p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0);
                }
            }
        }

        public override string name
        {
            get
            {
                return "botsummon";
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

