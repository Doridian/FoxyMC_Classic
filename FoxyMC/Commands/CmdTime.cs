namespace FoxyMC.Commands
{
    using System;

    public class CmdTime : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/time - Shows the server time.");
        }

        public override void Use(Player p, string message)
        {
            string str = DateTime.Now.ToString("hh:mm:ss tt");
            message = "Server time is " + str;
            p.SendMessage(message);
        }

        public override string name
        {
            get
            {
                return "time";
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

