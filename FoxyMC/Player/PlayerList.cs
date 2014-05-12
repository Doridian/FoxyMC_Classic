/*
	Copyright 2010 MCSharp Team Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
namespace FoxyMC
{
    public sealed class PlayerList
    {
        private Dictionary<string, Group> players = new Dictionary<string, Group>();

        public List<string> All()
        {
            return new List<string>(this.players.Keys);
        }

        public List<string> AllGroup(string g)
        {
            return AllGroup(Group.Find(g));
        }

        public List<string> AllGroup(Group g)
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, Group> kv in players)
            {
                if (kv.Value.Equals(g)) l.Add(kv.Key);
            }
            return l;
        }

        public List<string> AllMin(LevelPermission rank)
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, Group> kv in players)
            {
                if (kv.Value.Permission >= rank) l.Add(kv.Key);
            }
            return l;
        }

        public List<string> AllExact(LevelPermission rank)
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, Group> kv in players)
            {
                if (kv.Value.Permission == rank) l.Add(kv.Key);
            }
            return l;
        }

        public Group GetGroup(Player p)
        {
            return GetGroup(p.name);
        }

        public Group GetGroup(string p)
        {
            p = p.ToLower();
            if (!this.players.ContainsKey(p)) return Group.standard;
            return this.players[p];
        }

        public void SetGroup(Player p, Group grp)
        {
            SetGroup(p.name, grp);
        }

        public void SetGroup(string p, Group grp)
        {
            p = p.ToLower();
            Player px = Player.Find(p);
            if (px != null) { px.group = grp; px.color = px.group.color; }
            if (!this.players.ContainsKey(p)) { this.players.Add(p, grp); return; }
            this.players[p] = grp;
        }

        public static PlayerList Load()
        {
            PlayerList list = new PlayerList();
            if (!File.Exists("ranks.txt")) return list;
            foreach (string str in File.ReadAllLines("ranks.txt"))
            {
                string[] split = str.Split('=');
                if (split.Length != 2) continue;
                list.SetGroup(split[0].ToLower(), Group.Find(split[1]));
            }
            return list;
        }

        public bool Remove(string p)
        {
            return this.players.Remove(p.ToLower());
        }

        public void Save()
        {
            this.Save(true);
        }

        public void Save(bool console)
        {
            StreamWriter file = File.CreateText("ranks.txt");
            foreach (KeyValuePair<string, Group> kv in players)
            {
                if (Group.standard != kv.Value) file.WriteLine(kv.Key + "=" + kv.Value.name);
            }
            file.Close();
            if (console)
            {
                Server.s.Log("SAVED: ranks.txt");
            }
        }
    }
}

