using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;

namespace FoxyMC.Commands
{
	public class CmdAdvert : Command
	{
        private List<string> adverts = new List<string>();
        private int lastadvert = 0;
        private Timer timer = new Timer();

        private void SendAdvert(object x, ElapsedEventArgs e)
        {
            if (adverts.Count < 1) return;
            Player.GlobalMessage("[&4ADVERT&e] " + adverts[lastadvert]);
            lastadvert++;
            if (lastadvert >= adverts.Count) lastadvert = 0;
        }

        public CmdAdvert()
        {
            lastadvert = 0;
            adverts.Clear();
            if (!File.Exists("adverts.txt")) return;
            adverts.AddRange(File.ReadAllLines("adverts.txt"));
            Server.s.Log(adverts.Count + " adverts loaded!");
            timer = new Timer(30 * 60000);
            timer.Elapsed += new ElapsedEventHandler(SendAdvert);
            timer.Start();
        }

        private void SaveAdv()
        {
            File.WriteAllLines("adverts.txt", adverts.ToArray());
        }

        public override void Help(Player p)
        {
            p.SendMessage("/advert <list/add/del> [text] - Manages adverts.");
        }

        public override void Use(Player p, string message)
        {
            int index = message.IndexOf(' ');
            string cmd = message.ToLower();
            string str2 = "";
            if (index > 0)
            {
                cmd = cmd.Substring(0, index);
                str2 = message.Substring(index + 1);
            }
            else if (cmd != "list")
            {
                this.Help(p);
                return;
            }

            switch (cmd)
            {
                case "add":
                    adverts.Add(str2);
                    p.SendMessage("Advert added!");
                    this.SaveAdv();
                    break;
                case "list":
                    p.SendMessage("Current adverts: ");
                    int imax = adverts.Count;
                    for (int i = 0; i < imax; i++)
                    {
                        p.SendMessage("[" + i + "] " + adverts[i]);
                    }
                    p.SendMessage("END OF LIST");
                    break;
                case "del":
                    adverts.RemoveAt(Convert.ToInt32(str2));
                    p.SendMessage("Advert removed!");
                    this.SaveAdv();
                    break;
                default:
                    this.Help(p);
                    return;
            }
        }

        public override string name
        {
            get
            {
                return "advert";
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
