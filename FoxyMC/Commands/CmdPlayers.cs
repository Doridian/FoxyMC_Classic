namespace FoxyMC.Commands
{
    using System;

    internal class CmdPlayers : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/players - Shows name and general rank of all players");
        }

        public override void Use(Player p, string message)
        {
            p.SendMessage("There are " + Player.number + " players online");
            string str = "";
            string str2 = "";
            string str3 = "";
            foreach (Player player in Player.players)
            {
                string str4;
                if (!player.hidden && ((str4 = player.group.name.ToLower()) != null))
                {
                    if (!(str4 == "guest"))
                    {
                        if ((str4 == "operator") || (str4 == "superop"))
                        {
                            goto Label_00CE;
                        }
                        if ((str4 == "builder") || (str4 == "advbuilder"))
                        {
                            goto Label_00E7;
                        }
                    }
                    else
                    {
                        str2 = str2 + " " + player.name + ",";
                    }
                }
                continue;
            Label_00CE:
                str = str + " " + player.name + ",";
                continue;
            Label_00E7:
                str3 = str3 + " " + player.name + ",";
            }
            p.SendMessage(":&3Operators&e:" + str.Trim(new char[] { ',' }));
            p.SendMessage(":&2Builders&e:" + str3.Trim(new char[] { ',' }));
            p.SendMessage(":&7Guests&e:" + str2.Trim(new char[] { ',' }));
        }

        public override string name
        {
            get
            {
                return "players";
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

