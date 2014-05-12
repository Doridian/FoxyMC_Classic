namespace FoxyMC.Commands
{
    using System;

    public class CmdHelp : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/help [command] - Shows a list of commands or more detail for a specific command.");
        }

        public override void Use(Player p, string message)
        {
            message.ToLower();
            if (message == "")
            {
                foreach (Command cmd in Command.all.Values)
                {
                    try
                    {
                        if (cmd.level <= p.group.Permission) message = message + ", " + cmd.name;
                    }
                    catch
                    {
                    }
                }
                p.SendMessage("Available commands: " + message.Remove(0, 2) + ". For more info about a specific command write \"/help <command>\".");
            }
            else
            {
                if (!Command.all.ContainsKey(message))
                {
                    p.SendMessage("There is no command \"" + message + "\"");
                }
                else
                {
                    Command command = Command.all[message];
                    command.Help(p);
                }
            }
        }

        public override string name
        {
            get
            {
                return "help";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Banned;
            }
        }
    }
}

