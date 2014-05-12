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

    public abstract class Group
    {
        public static List<Group> GroupList = new List<Group>();
        public static Group standard;

        protected Group()
        {
        }

        public bool CanExecute(Command cmd)
        {
            return (cmd.level <= this.Permission);
        }

        public string GetTag()
        {
            return this.color + this.name + "&e";
        }

        public static bool Exists(string name)
        {
            name = name.ToLower();
            foreach (Group group in GroupList)
            {
                if (group.name == name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static Group Find(string name)
        {
            name = name.ToLower();
            foreach (Group group in GroupList)
            {
                if (group.name == name)
                {
                    return group;
                }
            }
            return Group.standard;
        }

        public static void InitAll()
        {
            GroupList.Clear();
            foreach (Type t in AssemblyLoader.GetAllClasses("Groups"))
            {
                try
                {
                    Group g = (Group)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    GroupList.Add(g);
                    //Console.Out.WriteLine("Added group: " + g.name);
                }
                catch (Exception) { }
            }

            standard = Group.Find("guest");

            try
            {
                foreach (Player p in Player.players)
                {
                    p.group = Group.Find(p.group.name);
                    p.color = p.group.color;
                }
            }
            catch (Exception) { }
        }

        public abstract bool canChat { get; }

        public abstract string color { get; }

        public abstract string name { get; }

        public abstract LevelPermission Permission { get; }
    }
}

