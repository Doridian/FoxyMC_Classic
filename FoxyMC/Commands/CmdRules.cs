namespace FoxyMC.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class CmdRules : Command
    {
        public override void Help(Player p)
        {
            if ((p.group.name != "operator") && (p.group.name != "superop"))
            {
                p.SendMessage("/rules - Displays server rules");
            }
            else
            {
                p.SendMessage("/rules [player]- Displays server rules to a player");
            }
        }

        public override void Use(Player p, string message)
        {
            List<string> list = new List<string>();
            if (!File.Exists("rules.txt"))
            {
                p.SendMessage("There is no rules.txt file to show.");
            }
            else
            {
                StreamReader reader = File.OpenText("rules.txt");
                while (!reader.EndOfStream)
                {
                    list.Add(reader.ReadLine());
                }
                reader.Close();
                Player player = null;
                if (message != "")
                {
                    if ((p.group == Group.Find("guest")) | (p.group == Group.Find("banned")))
                    {
                        p.SendMessage("You cant send /rules to another player!");
                        return;
                    }
                    player = Player.Find(message);
                }
                else
                {
                    player = p;
                }
                if (player != null)
                {
                    if ((player.level == Server.mainLevel) && (Server.mainLevel.permissionbuild == LevelPermission.Guest))
                    {
                        player.SendMessage("You are currently on the guest map where anyone can build");
                    }
                    player.SendMessage("Server Rules:");
                    foreach (string str in list)
                    {
                        player.SendMessage(str);
                    }
                }
                else
                {
                    p.SendMessage("There is no player \"" + message + "\"!");
                }
            }
        }

        public override string name
        {
            get
            {
                return "rules";
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

