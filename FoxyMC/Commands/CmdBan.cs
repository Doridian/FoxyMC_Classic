namespace FoxyMC.Commands
{
    using System;

    public class CmdBan : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/ban <player> - Bans a player without kicking him.");
            p.SendMessage("Add # before name to stealth ban.");
        }

        public override void Use(Player p, string message)
        {
            bool flag = false;
            if (message == "")
            {
                this.Help(p);
                return;
            }
            if (message[0] == '#')
            {
                message = message.Remove(0, 1).Trim();
                flag = true;
                Server.s.Log("Stealth Ban Atempted");
            }

            if(p.group.Permission <= Server.ranks.GetGroup(message).Permission)
            {
                p.SendMessage("Permission denied!");
                return;
            }

            Server.ranks.SetGroup(message, Group.Find("banned"));
            Server.ranks.Save();

            Player player = Player.Find(message);
            if (player == null)
            {
                Player.GlobalMessage(message + " &f(offline)&e is now &8banned&e!");
            }
            else
            {
                if (flag)
                {
                    Player.GlobalMessageOps(player.color + player.name + "&e is now STEALTH &8banned&e!");
                }
                else
                {
                    Player.GlobalChat(player, player.color + player.name + "&e is now &8banned&e!", false);
                }
                player.color = player.group.color;
                Player.GlobalDie(player, false);
                Player.GlobalSpawn(player, player.pos[0], player.pos[1], player.pos[2], player.rot[0], player.rot[1], false);
            }
            Server.s.Log("BANNED: " + message.ToLower());
            /*if (p != null)
            {

                else if (!Player.ValidName(message))
                {
                    p.SendMessage("Invalid name \"" + message + "\".");
                }
                else if (Server.operators.Contains(message))
                {
                    p.SendMessage("You can't ban an operator!");
                }
                else
                {
                    if (Server.builders.Contains(message))
                    {
                        Server.builders.Remove(message);
                        Server.builders.Save("builders.txt");
                    }
                    if (Server.advbuilders.Contains(message))
                    {
                        Server.advbuilders.Remove(message);
                        Server.advbuilders.Save("advbuilders.txt");
                    }
                    if (Server.superOps.Contains(message))
                    {
                        p.SendMessage("You can't ban a Super Op!");
                    }
                    else if (Server.banned.Contains(message))
                    {
                        p.SendMessage(message + " is already banned.");
                    }
                    else
                    {

                }
            }
            else if ((message != "") && (Player.ValidName(message) && !Server.operators.Contains(message)))
            {
                if (Server.builders.Contains(message))
                {
                    Server.builders.Remove(message);
                    Server.builders.Save("builders.txt");
                }
                if (!Server.superOps.Contains(message) && !Server.banned.Contains(message))
                {
                    Player from = Player.Find(message);
                    if (from == null)
                    {
                        Player.GlobalMessage(message + " &f(offline)&e is now &8banned&e!");
                    }
                    else
                    {
                        if (flag)
                        {
                            Player.GlobalMessageOps(from.color + from.name + "&e is now STEALTH &8banned&e!");
                        }
                        else
                        {
                            Player.GlobalChat(from, from.color + from.name + "&e is now &8banned&e!", false);
                        }
                        from.group = Group.Find("banned");
                        from.color = from.group.color;
                        Player.GlobalDie(from, false);
                        Player.GlobalSpawn(from, from.pos[0], from.pos[1], from.pos[2], from.rot[0], from.rot[1], false);
                    }
                    Server.banned.Add(message);
                    Server.banned.Save("banned.txt", false);
                    Server.s.Log("BANNED: " + message.ToLower());
                }
            }*/
        }

        public override string name
        {
            get
            {
                return "ban";
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

