namespace FoxyMC.Commands
{
    using System;

    public class CmdBotRemove : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/botremove <name> - Remove a bot on the same level as you");
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
                    p.SendMessage("There is no bot " + bot + "!");
                }
                else if (p.level != bot.level)
                {
                    p.SendMessage(bot.name + " is in a different level.");
                }
                else
                {
                    bot.GlobalDie();
                }
            }
        }

        public override string name
        {
            get
            {
                return "botremove";
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

