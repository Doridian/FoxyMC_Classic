namespace FoxyMC.Commands
{
    using System;

    internal class CmdGM : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/gm <cmd> - Gamemode specific commands");
        }

        public override void Use(Player p, string message)
        {
            message = message.Trim();
            int index = message.IndexOf(' ');
            string cmd = message.ToLower();
            string args = "";
            if (index > 0)
            {
                cmd = cmd.Substring(0, index);
                args = message.Substring(index + 1);
            }
            if (cmd == "help")
            {
                p.level.gamemode.Help(p);
            }
            else
            {
                if (!p.level.gamemode.RunCmd(p, cmd, args))
                {
                    p.SendMessage("Unknown GameMode command!");
                }
            }
        }

        public override string name
        {
            get
            {
                return "gm";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Guest;
            }
        }
    }
}

