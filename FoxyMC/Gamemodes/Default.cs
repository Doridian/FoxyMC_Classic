using System;
using System.Collections.Generic;
using System.Text;

namespace FoxyMC.Gamemodes
{
    class Default : Gamemode
    {
        public override void Help(Player p)
        {
            //Well, who doesn't know how to build, IS FUCKED ANYWAYS xD
        }

        public override void Start()
        {
            //We neither start anything
        }

        public override void Stop()
        {
            //nor to stop anything ;)
        }

        protected override void InitOptions()
        {
            //This doesn't have any options...
        }

        public override string name
        {
            get { return "default"; }
        }

        public override bool RunCmd(Player p, string cmd, string args)
        {
            return false;
        }

        public override bool Blockchange(Player p, ushort x, ushort y, ushort z, byte oldtype, byte newtype)
        {
            return true;
        }
    }
}
