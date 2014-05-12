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

namespace FoxyMC.Groups
{
    public class GrpBuilder : Group
    {
        public override LevelPermission Permission { get { return LevelPermission.Builder; } }
        public override string name { get { return "builder"; } }
        public override string color { get { return "&a"; } }
        public override bool canChat { get { return true; } }
    }
}