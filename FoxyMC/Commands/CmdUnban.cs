namespace FoxyMC.Commands
{
    using System;

    public class CmdUnban : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/unban <player> - Unbans a player.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else if (!Player.ValidName(message))
            {
                p.SendMessage("Invalid name \"" + message + "\".");
            }
            else if (Server.ranks.GetGroup(message).Permission != LevelPermission.Banned)
            {
                p.SendMessage(message + " isn't banned.");
            }
            else
            {
                Player from = Player.Find(message);
                if (from == null)
                {
                    Player.GlobalMessage(message + " &8(banned)&e is now " + Group.standard.color + Group.standard.name + "&e!");
                }
                else
                {
                    Player.GlobalChat(from, from.color + from.name + "&e is now " + Group.standard.color + Group.standard.name + "&e!", false);
                    from.group = Group.standard;
                    from.color = from.group.color;
                    Player.GlobalDie(from, false);
                    Player.GlobalSpawn(from, from.pos[0], from.pos[1], from.pos[2], from.rot[0], from.rot[1], false);
                }
                Server.ranks.SetGroup(message, Group.standard);
                Server.s.Log("UNBANNED: " + message.ToLower());
            }
        }

        public override string name
        {
            get
            {
                return "unban";
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

