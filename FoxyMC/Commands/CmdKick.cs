namespace FoxyMC.Commands
{
    using System;

    public class CmdKick : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/kick <player> [message] - Kicks a player.");
        }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                if (message == "")
                {
                    this.Help(p);
                }
                else
                {
                    string name = message;
                    int index = message.IndexOf(' ');
                    if (index != -1)
                    {
                        name = message.Substring(0, index);
                        message = message.Substring(index + 1);
                    }
                    if (Player.Exists(name))
                    {
                        Player player = Player.Find(name);
                        if (player == p)
                        {
                            p.SendMessage("You can't kick yourself!");
                        }
                        else if ((player.group == Group.Find("operator")) && (p.group != Group.Find("superOp")))
                        {
                            p.SendMessage("You can't kick an operator!");
                        }
                        else if (player.group == Group.Find("superOp"))
                        {
                            p.SendMessage("You can't kick a Super Op!");
                        }
                        else if (index == -1)
                        {
                            player.Kick("You were kicked by " + p.name + "!");
                        }
                        else
                        {
                            player.Kick(message);
                        }
                    }
                    else
                    {
                        p.SendMessage("There is no player \"" + name + "\"!");
                    }
                }
            }
            else if (message != "")
            {
                string str2 = message;
                int length = message.IndexOf(' ');
                if (length != -1)
                {
                    str2 = message.Substring(0, length);
                    message = message.Substring(length + 1);
                }
                if (Player.Exists(str2))
                {
                    Player player2 = Player.Find(str2);
                    if ((player2.group != Group.Find("operator")) && (player2.group != Group.Find("superOp")))
                    {
                        if (length == -1)
                        {
                            player2.Kick("You were kicked by [console]!");
                        }
                        else
                        {
                            player2.Kick(message);
                        }
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "kick";
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

