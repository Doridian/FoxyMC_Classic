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
using System.IO;
using System.Net;

namespace FoxyMC
{
    public static class Properties
    {
        public static void Load()
        {
            string rndchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            for (int i = 0; i < 16; ++i) { Server.salt += rndchars[rnd.Next(rndchars.Length)]; }

            if (File.Exists("server.properties"))
            {
                string[] lines = File.ReadAllLines("server.properties");

                foreach (string line in lines)
                {
                    if (line != "" && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = line.Split('=')[1].Trim();

                        switch (key.ToLower())
                        {
                            case "server-name":
                                if (ValidString(value, "![]:.,{}~-+()?_/\\ "))
                                {
                                    Server.name = value;
                                }
                                else
                                {
                                    Server.s.Log("server-name invalid! setting to default.");
                                }
                                break;
                            case "motd":
                                if (ValidString(value, "![]&:.,{}~-+()?_/\\ "))
                                {
                                    Server.motd = value;
                                }
                                else
                                {
                                    Server.s.Log("motd invalid! setting to default.");
                                }
                                break;
                            case "port":
                                try
                                {
                                    Server.port = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.s.Log("port invalid! setting to default.");
                                }
                                break;
                            /*case "ip":
                                try
                                {
                                    Server.ip = IPAddress.Parse(value);
                                }
                                catch
                                {
                                    Server.s.Log("ip invalid! setting to default.");
                                }
                                break;*/
                            case "verify-names":
                                Server.verify = (value.ToLower() == "true") ? true : false;
                                break;
                            case "public":
                                Server.pub = (value.ToLower() == "true") ? true : false;
                                break;
                            case "world-chat":
                                Server.worldChat = (value.ToLower() == "true") ? true : false;
                                break;
                            case "guest-goto":
                                Server.guestGoto = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-players":
                                try
                                {
                                    if (Convert.ToByte(value) > 64)
                                    {
                                        value = "64";
                                        Server.s.Log("Max players has been lowered to 64.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                        Server.s.Log("Max players has been increased to 1.");
                                    }
                                    Server.players = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.s.Log("max-players invalid! setting to default.");
                                }
                                break;
                            case "max-maps":
                                try
                                {
                                    if (Convert.ToByte(value) > 20)
                                    {
                                        value = "20";
                                        Server.s.Log("Max maps has been lowered to 20.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                        Server.s.Log("Max maps has been increased to 1.");
                                    }
                                    Server.maps = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.s.Log("max-maps invalid! setting to default.");
                                }
                                break;
                            case "irc":
                                Server.irc = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-server":
                                Server.ircServer = value;
                                break;
                            case "irc-nick":
                                Server.ircNick = value;
                                break;
                            case "irc-channel":
                                Server.ircChannel = value;
                                break;
                            case "irc-port":
                                try
                                {
                                    Server.ircPort = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.s.Log("irc-port invalid! setting to default.");
                                }
                                break;
                            case "irc-identify":
                                try
                                {
                                    Server.ircIdentify = Convert.ToBoolean(value);
                                }
                                catch
                                {
                                    Server.s.Log("irc-identify boolean value invalid! Setting to the default of: " + Server.ircIdentify + ".");
                                }
                                break;
                            case "irc-password":
                                Server.ircPassword = value;
                                break;
                            case "anti-tunnels":
                                Server.antiTunnel = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-depth":
                                try
                                {
                                    Server.maxDepth = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.s.Log("maxDepth invalid! setting to default.");
                                }
                                break;

                            case "overload":
                                try
                                {
                                    if (Convert.ToInt16(value) > 5000)
                                    {
                                        value = "4000";
                                        Server.s.Log("Max overload is 5000.");
                                    }
                                    else if (Convert.ToInt16(value) < 500)
                                    {
                                        value = "500";
                                        Server.s.Log("Min overload is 500");
                                    }
                                    Server.Overload = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    Server.s.Log("Overload invalid! setting to default.");
                                }
                                break;
                            case "report-back":
                                Server.reportBack = (value.ToLower() == "true") ? true : false;
                                break;
                            case "backup-time":
                                if (Convert.ToInt32(value)>1)
                                {
                                    Server.backupInterval = Convert.ToInt32(value);
                                }
                                break;
                            case "console-only":
                                Server.console = (value.ToLower() == "true") ? true : false;
                                break;
                            case "maps-url":
                                Server.mapimg = value;
                                break;

                        }
                    }
                } 
                Server.s.Log("LOADED: server.properties");
                Server.s.SettingsUpdate();
                Save();
            }
            else
                Save();
        }
        public static bool ValidString(string str, string allowed)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890" + allowed;
            foreach (char ch in str)
            {
                if (allowedchars.IndexOf(ch) == -1)
                {
                    return false;
                }
            } return true;
        }

        static void Save()
        {
            try
            {
                StreamWriter w = new StreamWriter(File.Create("server.properties"));
                w.WriteLine("# Edit the settings below to modify how your server operates. This is an explanation of what each setting does.");
                w.WriteLine("#   server-name\t=\tThe name which displays on minecraft.net");
                w.WriteLine("#   motd\t=\tThe message which displays when a player connects");
                w.WriteLine("#   port\t=\tThe port to operate from");
                w.WriteLine("#   console-only\t=\tRun without a GUI (useful for Linux servers with mono)");
                w.WriteLine("#   verify-names\t=\tVerify the validity of names");
                w.WriteLine("#   public\t=\tSet to true to appear in the public server list");
                w.WriteLine("#   max-players\t=\tThe maximum number of connections");
                w.WriteLine("#   max-maps\t=\tThe maximum number of maps loaded at once");
                w.WriteLine("#   world-chat\t=\tSet to true to enable world chat");
                w.WriteLine("#   guest-goto\t=\tSet to true to give guests goto and levels commands");
                w.WriteLine("#   irc\t=\tSet to true to enable the IRC bot");
                w.WriteLine("#   irc-nick\t=\tThe name of the IRC bot");
                w.WriteLine("#   irc-server\t=\tThe server to connect to");
                w.WriteLine("#   irc-channel\t=\tThe channel to join");
                w.WriteLine("#   irc-port\t=\tThe port to use to connect");
                w.WriteLine("#   irc-identify\t=(true/false)\tDo you want the IRC bot to Identify itself with nickserv. Note: You will need to register it's name with nickserv manually.");
                w.WriteLine("#   irc-password\t=\tThe password you want to use if you're identifying with nickserv");
                w.WriteLine("#   anti-tunnels\t=\tStops people digging below max-depth");
                w.WriteLine("#   max-depth\t=\tThe maximum allowed depth to dig down");
                w.WriteLine("#   backup-time\t=\tThe number of seconds between automatic backups");
                w.WriteLine("#   overload\t=\tThe higher this is, the longer the physics is allowed to lag. Default 1500");
                w.WriteLine("#   report-back\t=\tAutomatically report crash information back to MCSharp developers (not yet in use)");
                w.WriteLine("#   maps-url\t=\tURL for map renders...");
                w.WriteLine();
                w.WriteLine();
                w.WriteLine("# Server options");
                w.WriteLine("server-name = " + Server.name);
                w.WriteLine("motd = " + Server.motd);
                w.WriteLine("port = " + Server.port.ToString());
                w.WriteLine("console-only = " + Server.console.ToString().ToLower());
                w.WriteLine("verify-names = " + Server.verify.ToString().ToLower());
                w.WriteLine("public = " + Server.pub.ToString().ToLower());
                w.WriteLine("max-players = " + Server.players.ToString());
                w.WriteLine("max-maps = " + Server.maps.ToString());
                w.WriteLine("world-chat = " + Server.worldChat.ToString().ToLower());
                w.WriteLine("guest-goto = " + Server.guestGoto.ToString().ToLower());
                w.WriteLine("maps-url = " + Server.mapimg);
                w.WriteLine();
                w.WriteLine("# irc bot options");
                w.WriteLine("irc = " + Server.irc.ToString().ToLower());
                w.WriteLine("irc-nick = " + Server.ircNick);
                w.WriteLine("irc-server = " + Server.ircServer);
                w.WriteLine("irc-channel = " + Server.ircChannel);
                w.WriteLine("irc-port = " + Server.ircPort.ToString());
                w.WriteLine("irc-identify = " + Server.ircIdentify.ToString());
                w.WriteLine("irc-password = " + Server.ircPassword);
                w.WriteLine();
                w.WriteLine("# other options");
                w.WriteLine("anti-tunnels = " + Server.antiTunnel.ToString().ToLower());
                w.WriteLine("max-depth = " + Server.maxDepth.ToString());
                w.WriteLine("overload = " + Server.Overload.ToString());
                w.WriteLine();
                w.WriteLine("# backup options");
                w.WriteLine("backup-time = 150");
                w.WriteLine();
                w.WriteLine("#Error logging");
                w.WriteLine("report-back = " + Server.reportBack.ToString().ToLower());
                w.Flush();
                w.Close();

                Server.s.Log("SAVED: server.properties");
            }
            catch
            {
                Server.s.Log("SAVE FAILED! server.properties");
            }
        }
    }
}
