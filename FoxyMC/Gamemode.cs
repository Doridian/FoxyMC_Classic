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
using System.Timers;

namespace FoxyMC
{
    public abstract class TickBasedGamemode : Gamemode
    {
        Timer timer;

        public abstract void TimerTick(object t, ElapsedEventArgs e);

        public override void Start()
        {
            if (timer != null) timer.Stop();
            timer = new Timer(100);
            timer.Elapsed += new ElapsedEventHandler(TimerTick);
            timer.Start();
        }

        public override void Stop()
        {
            if (timer != null) timer.Stop();
        }
    }

    public abstract class Gamemode
    {
        //public static CommandList all = new CommandList();
        public static Type standard;

        public static Dictionary<string, Type> all = new Dictionary<string, Type>();

        protected Gamemode()
        {

        }

        protected Level level;

        public abstract bool Blockchange(Player p, ushort x, ushort y, ushort z, byte oldtype, byte newtype);

        public abstract void Help(Player p);

        public abstract bool RunCmd(Player p, string cmd, string args);

        public static void InitAll()
        {
            all.Clear();
            foreach (Type t in AssemblyLoader.GetAllClasses("Gamemodes"))
            {
                try
                {
                    string gn = ((Gamemode)t.GetConstructor(System.Type.EmptyTypes).Invoke(null)).name;
                    all.Add(gn, t);
                    if (gn == "default") standard = t;
                }
                catch (Exception) { }
            }

            try
            {
                foreach (Level l in Server.levels)
                {
                    Gamemode.SetGamemode(l, l.gamemode.name);
                }
            }
            catch (Exception) { }
        }

        public static void SetGamemode(Level l, string gamemode)
        {
            gamemode = gamemode.ToLower();
            Type t = standard;
            if (all.ContainsKey(gamemode)) t = all[gamemode];
            Gamemode gm = (Gamemode)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);

            try
            {
                l.gamemode.Stop();
            }
            catch (Exception) { }

            l.gamemode = gm;
            gm.level = l;
            gm.Init();
            gm.Start();
        }

        public static void Load(Level level)
        {
            string fld = "gamemodes/" + level.name + "/CURRENT.cfg";
            string gamemode = "default";
            if (File.Exists(fld))
            {
                gamemode = File.ReadAllText(fld);
            }
            SetGamemode(level, gamemode);
        }

        public static void Save(Level level)
        {
            if (!Directory.Exists("gamemodes/" + level.name)) Directory.CreateDirectory("gamemodes/" + level.name);
            File.WriteAllText("gamemodes/" + level.name + "/CURRENT.cfg", level.gamemode.name);
            StreamWriter f = File.CreateText("gamemodes/" + level.name + "/" + level.gamemode.name + ".txt");
            foreach (KeyValuePair<string, object> kv in level.gamemode.options)
            {
                f.WriteLine(kv.Key + "=" + kv.Value.ToString());
            }
            f.Close();
        }

        private void Init()
        {
            this.InitOptions();
            string fld = "gamemodes/" + level.name + "/" + this.name + ".txt";
            if (File.Exists(fld))
            {
                foreach (string s in File.ReadAllLines(fld))
                {
                    string[] spl = s.Split(new char[] { '=' }, 2);
                    if (spl.Length != 2) continue;
                    this.SetOption(spl[0], spl[1]);
                }
            }
        }

        protected abstract void InitOptions();

        public bool SetOption(string key, object value)
        {
            try
            {
                if (!options.ContainsKey(key)) return false;
                value = Convert.ChangeType(value, options[key].GetType());
                if (value == null) return false;
                options[key] = value;
                return true;
            }
            catch (Exception) { return false; }
        }

        public object GetOption(string key)
        {
            if (!options.ContainsKey(key)) return null;
            return options[key];
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract string name { get; }

        protected Dictionary<string, object> options = new Dictionary<string, object>();
    }
}

