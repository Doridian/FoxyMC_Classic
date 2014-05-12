namespace FoxyMC.Commands
{
    using System;

    public class CmdKickban : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/kickban <player> [message] - Kicks and bans a player with an optional message.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                string name = message;
                int index = message.IndexOf(' ');
                string str2 = "";
                if (index != -1)
                {
                    name = message.Substring(0, index);
                    str2 = message.Substring(index + 1);
                }
                Player player = Player.Find(name);

                if(Server.ranks.GetGroup(name).Permission >= p.group.Permission)
                {
                    p.SendMessage("Permission denied!");
                    return;
                }

                Server.ranks.SetGroup(name, Group.Find("banned"));

                if (player != null)
                {
                    if (player == p)
                    {
                        p.SendMessage("You can't kickban yourself!");
                    }
                    else
                    {
                        if (index == -1)
                        {
                            player.Kick("You were kickbanned by " + p.name + "!");
                        }
                        else
                        {
                            player.Kick(str2);
                        }       
                    }
                }
                Server.s.Log("KICKBANNED: " + message.ToLower());
            }
        }

        public override string name
        {
            get
            {
                return "kickban";
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

