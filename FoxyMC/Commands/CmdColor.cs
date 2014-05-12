namespace FoxyMC.Commands
{
    using System;

    public class CmdColor : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/color [player] <color> - Changes the nick color.");
            p.SendChat(p, "&0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            p.SendChat(p, "&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
        }

        private static string Name(string name)
        {
            char ch = name[name.Length - 1];
            string str = ch.ToString().ToLower();
            if (!(str == "s") && !(str == "x"))
            {
                return (name + "&e's");
            }
            return (name + "&e'");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else if (message.Split(new char[] { ' ' }).Length > 2)
            {
                this.Help(p);
            }
            else
            {
                int index = message.IndexOf(' ');
                if (index != -1)
                {
                    Player from = Player.Find(message.Substring(0, index));
                    if (from == null)
                    {
                        p.SendMessage("There is no player \"" + message.Substring(0, index) + "\"!");
                    }
                    else
                    {
                        string str = c.Parse(message.Substring(index + 1));
                        if (str == "")
                        {
                            p.SendMessage("There is no color \"" + message + "\".");
                        }
                        else if (str == from.color)
                        {
                            p.SendMessage(from.name + " already has that color.");
                        }
                        else
                        {
                            Player.GlobalChat(from, from.color + "*" + Name(from.name) + " color changed to " + str + c.Name(str) + "&e.", false);
                            from.color = str;
                            Player.GlobalDie(from, false);
                            Player.GlobalSpawn(from, from.pos[0], from.pos[1], from.pos[2], from.rot[0], from.rot[1], false);
                        }
                    }
                }
                else
                {
                    string str2 = c.Parse(message);
                    if (str2 == "")
                    {
                        p.SendMessage("There is no color \"" + message + "\".");
                    }
                    else if (str2 == p.color)
                    {
                        p.SendMessage("You already have that color.");
                    }
                    else
                    {
                        Player.GlobalChat(p, p.color + "*" + Name(p.name) + " color changed to " + str2 + c.Name(str2) + "&e.", false);
                        p.color = str2;
                        Player.GlobalDie(p, false);
                        Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                    }
                }
            }
        }

        public override string name
        {
            get
            {
                return "color";
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

