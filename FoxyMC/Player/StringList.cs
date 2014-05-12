using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FoxyMC
{
    public sealed class StringList
    {
        // Fields
        private List<string> players = new List<string>();

        // Methods
        public void Add(string p)
        {
            this.players.Add(p.ToLower());
        }

        public List<string> All()
        {
            return new List<string>(this.players);
        }

        public bool Contains(string p)
        {
            return this.players.Contains(p.ToLower());
        }

        public static StringList Load(string path)
        {
            StringList list = new StringList();
            if (File.Exists(path))
            {
                foreach (string str in File.ReadAllLines(path))
                {
                    list.Add(str);
                }
                return list;
            }
            File.Create(path).Close();
            Server.s.Log("CREATED NEW: " + path);
            return list;
        }

        public bool Remove(string p)
        {
            return this.players.Remove(p.ToLower());
        }

        public void Save(string path)
        {
            this.Save(path, true);
        }

        public void Save(string path, bool console)
        {
            StreamWriter file = File.CreateText(path);
            this.players.ForEach(delegate(string p)
            {
                file.WriteLine(p);
            });
            file.Close();
            if (console)
            {
                Server.s.Log("SAVED: " + path);
            }
        }
    }
}
