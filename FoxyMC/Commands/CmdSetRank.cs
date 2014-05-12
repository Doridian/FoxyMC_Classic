namespace FoxyMC.Commands
{
    using System;

    public class CmdSetRank : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/setrank <player> <rank> - Sets or returns a players rank.");
            p.SendMessage("You may use /rank as a shortcut");
            p.SendMessage("Valid Ranks are: guest, builder, adv, advbuilder, op, operator");
        }

        public override void Use(Player p, string message)
        {
            string[] split = message.Split(new char[] { ' ' });
            if (split.Length == 1)
            {
                Command.all["whois"].Use(p, message);
            }
            else if (split.Length == 2)
            {
                string name = split[0].ToLower();
                string str2 = split[1].ToLower();
                LevelPermission plevel = 0;
                LevelPermission newlevel = 0;
                LevelPermission oldlevel = 0;
                if (!Player.ValidName(name))
                {
                    p.SendMessage("Invalid name \"" + message + "\".");
                }
                else
                {
                    plevel = p.group.Permission;
                    oldlevel = Server.ranks.GetGroup(name).Permission;
                    switch (str2)
                    {
                        case "op":
                            str2 = "operator";
                            break;

                        case "adv":
                            str2 = "advbuilder";
                            break;
                    }

                    Group grp = Group.Find(str2);
                    if (grp == null)
                    {
                        p.SendMessage("Invalid rank");
                        return;
                    }

                    newlevel = grp.Permission;

                    if (oldlevel == LevelPermission.Banned)
                    {
                        p.SendMessage(name + " is banned! You cannot change a banned players rank!");
                    }
                    else if (plevel <= oldlevel || plevel <= newlevel)
                    {
                        p.SendMessage("Permission denied!");
                    }
                    else if (newlevel == oldlevel)
                    {
                        p.SendMessage(name + " is already the rank \"" + str2 + "\"");
                    }
                    else if (p.name == name)
                    {
                        p.SendMessage("You cannot modify your own rank!");
                    }
                    else
                    {
                        Player from = Player.Find(name);
                        Server.ranks.SetGroup(name, grp);
                        Server.ranks.Save();
                        string grptag = grp.GetTag();
                        if (from == null)
                        {
                            Player.GlobalMessage(name + " &f(offline)&e's rank was set to " + grptag + " by " + p.name + "!");
                        }
                        else
                        {
                            Player.GlobalChat(from, from.color + from.name + "&e's rank was set to " + grptag + " by " + p.name + "!", false);
                            Player.GlobalDie(from, false);
                            from.SendMessage("You are now ranked "+grptag+", type /help for your new set of commands.");
                            Player.GlobalSpawn(from, from.pos[0], from.pos[1], from.pos[2], from.rot[0], from.rot[1], false);
                        }
                        Server.s.Log("Promotion("+grp.name.ToUpper()+"): " + name + " promoted by " + p.name);
                        return;
                    }
                }
            }
            else
            {
                this.Help(p);
            }
        }

        public override string name
        {
            get
            {
                return "setrank";
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

