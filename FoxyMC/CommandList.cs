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

    public sealed class CommandList
    {
        private List<Command> commands = new List<Command>();

        public void Add(Command cmd)
        {
            this.commands.Add(cmd);
        }

        public List<Command> All()
        {
            return new List<Command>(this.commands);
        }

        public bool Contains(Command cmd)
        {
            return this.commands.Contains(cmd);
        }

        public bool Contains(string name)
        {
            name = name.ToLower();
            foreach (Command command in this.commands)
            {
                if (command.name == name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public Command Find(string name)
        {
            name = name.ToLower();
            foreach (Command command in this.commands)
            {
                if (command.name == name.ToLower())
                {
                    return command;
                }
            }
            return null;
        }

        public bool Remove(Command cmd)
        {
            return this.commands.Remove(cmd);
        }
    }
}

