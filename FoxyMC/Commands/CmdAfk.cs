namespace FoxyMC.Commands
{
    using System;

    internal class CmdAfk : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/afk <reason> - mark yourself as AFK. Use again to mark yourself as back");
        }

        public override void Use(Player p, string message)
        {
            if (message != "list")
            {
                if (Server.afkset.Contains(p.name))
                {
                    Server.afkset.Remove(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&e- is no longer AFK");
                }
                else
                {
                    Server.afkset.Add(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&e- is AFK " + message);
                }
            }
            else
            {
                foreach (string str in Server.afkset)
                {
                    p.SendMessage(str);
                }
            }
        }

        public override string name
        {
            get
            {
                return "afk";
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

