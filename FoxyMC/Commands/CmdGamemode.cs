namespace FoxyMC.Commands
{
    using System;

    public class CmdGamemode : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/gmode <start/stop/set/oset/oget/olist> [param1] [param2] - Gamemode Management.");
        }

        public override void Use(Player p, string message)
        {
            if(message == "") { this.Help(p); return; }
            string[] spl = message.Split(new char[] { ' ' }, 3);
            switch (spl[0].ToLower())
            {
                case "start":
                    p.level.gamemode.Start();
                    p.SendMessage("Gamemode started!");
                    break;
                case "stop":
                    p.level.gamemode.Stop();
                    p.SendMessage("Gamemode stopped!");
                    break;
                case "set":
                    if (spl.Length != 2) { this.Help(p); return; }
                    Gamemode.SetGamemode(p.level, spl[1]);
                    Gamemode.Save(p.level);
                    p.SendMessage("Gamemode changed to: "+p.level.gamemode.name+"!");
                    break;
                case "oset":
                    if (spl.Length != 3) { this.Help(p); return; }
                    if (!p.level.gamemode.SetOption(spl[1], spl[2]))
                    {
                        p.SendMessage("Error in changing option!");
                    }
                    else
                    {
                        Gamemode.Save(p.level);
                        p.SendMessage("Option changed!");
                    }
                    break;
                case "oget":
                    if (spl.Length != 2) { this.Help(p); return; }
                    object val = p.level.gamemode.GetOption(spl[1]);
                    if (val == null) { p.SendMessage("Invalid option!"); return; }
                    p.SendMessage(spl[1] + " = " + val.ToString());
                    break;
                case "olist":
                    //TODO
                    break;
                default:
                    this.Help(p);
                    break;
            }
        }

        public override string name
        {
            get
            {
                return "gmode";
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

