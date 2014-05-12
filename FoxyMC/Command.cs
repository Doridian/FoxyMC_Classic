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
namespace FoxyMC
{

    public abstract class Command
    {
        //public static CommandList all = new CommandList();

        public static Dictionary<string, Command> all = new Dictionary<string, Command>();

        protected Command()
        {
        }

        public abstract void Help(Player p);
        public static void InitAll()
        {
            all.Clear();
            foreach (Type t in AssemblyLoader.GetAllClasses("Commands"))
            {
                try
                {
                    Command c = (Command)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    all.Add(c.name, c);
                    //Console.Out.WriteLine("Loaded command: " + c.name);
                }
                catch (Exception) { }
            }
        }

        public abstract void Use(Player p, string message);

        public abstract string name { get; }

        public abstract LevelPermission level { get; }
    }
}

