namespace FoxyMC.Commands
{
    using System;

    internal class CmdDna : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/dna <name> - displays the DNA iformation of the specified player");
        }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                string data = DNA.GetData(p.name, TableData.ID);
                string str2 = DNA.GetData(p.name, TableData.IP);
                string str3 = DNA.GetData(p.name, TableData.Rank);
                p.SendMessage(p.name);
                p.SendMessage(data);
                p.SendMessage(str2);
                p.SendMessage(str3);
            }
            else
            {
                Player player = Player.Find(message);
                string str4 = DNA.GetData(player.name, TableData.ID);
                string str5 = DNA.GetData(player.name, TableData.IP);
                string str6 = DNA.GetData(player.name, TableData.Rank);
                p.SendMessage(player.name);
                p.SendMessage(str4);
                p.SendMessage(str5);
                p.SendMessage(str6);
            }
        }

        public override string name
        {
            get
            {
                return "dna";
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

