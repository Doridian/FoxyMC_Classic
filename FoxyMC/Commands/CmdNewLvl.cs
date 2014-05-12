namespace FoxyMC.Commands
{
    using System;

    internal class CmdNewLvl : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/newlvl <name> <x> <y> <z> <type> - creates a new level.");
            p.SendMessage("Valid types: island, mountains, forest, ocean, flat, pixel");
            p.SendMessage("Example: /newlvl newmap 128 64 128 island");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                this.Help(p);
            }
            else
            {
                string[] strArray = message.Split(new char[] { ' ' });
                if (strArray.Length == 5)
                {
                    switch (strArray[4])
                    {
                        case "flat":
                        case "pixel":
                        case "island":
                        case "mountains":
                        case "ocean":
                        case "forest":
                        {
                            string n = strArray[0];
                            try
                            {
                                new Level(n, Convert.ToUInt16(strArray[1]), Convert.ToUInt16(strArray[2]), Convert.ToUInt16(strArray[3]), strArray[4]).Save();
                            }
                            finally
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            Player.GlobalMessage("Level " + n + " created");
                            return;
                        }
                    }
                    p.SendMessage("Valid types: island, mountains, forest, ocean, flat, pixel");
                }
                else
                {
                    p.SendMessage("Not enough parameters! <name> <x> <y> <z> <type>");
                }
            }
        }

        public override string name
        {
            get
            {
                return "newlvl";
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

