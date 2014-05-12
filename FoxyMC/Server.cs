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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MonoTorrent.Client;

namespace FoxyMC
{
    public class Server
    {
        public delegate void LogHandler(string message);
        public delegate void HeartBeatHandler();
        public delegate void MessageEventHandler(string message);
        public delegate void PlayerListHandler(List<Player> playerList);
        public delegate void VoidHandler();

        public event LogHandler OnLog;
        public event HeartBeatHandler HeartBeatFail;
        public event MessageEventHandler OnURLChange;
        public event PlayerListHandler OnPlayerListChange;
        public event VoidHandler OnSettingsUpdate;


		public static string Version { get { return "1.0"; } }

        static Socket listen;
        static System.Diagnostics.Process process;
        static System.Timers.Timer updateTimer = new System.Timers.Timer(100);
        //static System.Timers.Timer heartbeatTimer = new System.Timers.Timer(60000);     //Every 45 seconds
        static System.Timers.Timer messageTimer = new System.Timers.Timer(60000 * 5);   //Every 5 mins

        //static Thread renderThread;

        


        public static PlayerList ranks;
        public static StringList bannedIP;
        public static StringList ircControllers;

        public static MapGenerator MapGen;


        //public static PerformanceCounter PCCounter;
        //public static PerformanceCounter ProcessCounter;

        public static Level mainLevel;
        public static List<Level> levels;
        public static List<string> afkset = new List<string>();
        public static List<string> messages = new List<string>();

        //Lua lua;
        #region Server Settings
        public const byte version = 7;
        public static string salt = "";

        public static string name = "Minecraft Server";
        public static string motd = "Welcome to my server!";
        public static byte players = 12;
        public static byte maps = 5;
        public static int port = 25565;
        //public static IPAddress ip = IPAddress.Any;
        public static bool pub = true;
        public static bool verify = false;
        public static bool worldChat = true;
        public static bool guestGoto = false;

        public static string level = "main";
        public static string errlog = "error.log";

        public static bool console = false;
        public static bool reportBack = true;

        public static bool irc = false;
        public static int ircPort = 6667;
        public static string ircNick = "MCsharp";
        public static string ircServer = "irc.esper.net";
        public static string ircChannel = "#changethis";
        public static bool ircIdentify = false;
        public static string ircPassword = "";

        public static bool antiTunnel = false;
        public static byte maxDepth = 4;
        public static int Overload = 1000;
        public static int backupInterval = 150;

        public static string mapimg = "";
        #endregion

        /*private static IPEndPoint __BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            return new IPEndPoint(ip, 0);
        }
        public static BindIPEndPoint BindIPEndPointCallback = new BindIPEndPoint(__BindIPEndPointCallback);*/


        public static MainLoop ml;
        public static Server s;
        public Server()
        {
            //lua = new Lua();
            //lua["server"] = this;
            ml = new MainLoop("server");
            Server.s = this;
            
        }
        public void Start(){
            Log("Starting Server");
            Properties.Load();
            Thread.Sleep(100);

            Gamemode.InitAll();
            Command.InitAll();
            Group.InitAll();

            ServerInterlink.Start();

            ml.Queue(delegate
            {
                levels = new List<Level>(Server.maps);
                MapGen = new MapGenerator();

                Random random = new Random();

                if (File.Exists("levels/" + Server.level + ".lvl"))
                {
                    mainLevel = Level.Load(Server.level);
                    if (mainLevel == null)
                    {
                        if (File.Exists("levels/" + Server.level + ".lvl.backup"))
                        {
                            Log("Atempting to load backup.");
                            File.Copy("levels/" + Server.level + ".lvl.backup", "levels/" + Server.level + ".lvl", true);
                            mainLevel = Level.Load(Server.level);
                            if (mainLevel == null)
                            {
                                Log("BACKUP FAILED!");
                                Console.ReadKey(); return;
                            }
                        }
                        else
                        {
                            Log("BACKUP NOT FOUND!");
                            Console.ReadKey(); return;
                        }

                    }
                }
                else
                {
                    Log("mainlevel not found");
                    mainLevel = new Level(Server.level, 128, 64, 128, "flat");

                    mainLevel.permissionvisit = LevelPermission.Guest;
                    mainLevel.permissionbuild = LevelPermission.Guest;
                    mainLevel.Save();
                }
                levels.Add(mainLevel);
            });

            ml.Queue(delegate {
                bannedIP = StringList.Load("banned-ip.txt");
                ranks = PlayerList.Load();

                ranks.SetGroup("flist", Group.Find("bots"));

                ranks.Save();

                ircControllers = StringList.Load("IRC_Controllers.txt");
            });


            ml.Queue(delegate
            {
                if (File.Exists("autoload.txt"))
                {
                    try
                    {
                        string[] lines = File.ReadAllLines("autoload.txt");
                        foreach (string line in lines)
                        {
                            //int temp = 0;
                            string _line = line.Trim();
                            try
                            {

                                if (_line == "") { continue; }
                                if (_line[0] == '#') { continue; }
                                int index = _line.IndexOf("=");

                                string key = line.Split('=')[0].Trim();
                                string value;
                                try
                                {
                                    value = line.Split('=')[1].Trim();
                                }
                                catch
                                {
                                    value = "0";
                                }

                                if (!key.Equals("main"))
                                {
                                    Command.all["load"].Use(null, key + " " + value);
                                }
                                else
                                {
                                    try
                                    {
                                        int temp = int.Parse(value);
                                        if (temp >= 0 && temp <= 2)
                                        {
                                            mainLevel.physics = temp;
                                        }
                                    }
                                    catch
                                    {
                                        Server.s.Log("Physics variable invalid");
                                    }
                                }


                            }
                            catch
                            {
                                Server.s.Log(_line + " failed.");
                            }
                        }
                    }
                    catch
                    {
                        Server.s.Log("autoload.txt error");
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else
                {
                    Log("autoload.txt does not exist");
                }
            });

            ml.Queue(delegate
            {
                s.Log("Creating listening socket on port "+Server.port+ "... ");
                if (Setup())
                {
                    s.Log("Done.");
                }
                else
                {
					s.Log("Could not create socket connection.  Shutting down.");
                    return;
                }
            });

            ml.Queue(delegate
            {
                updateTimer.Elapsed += delegate
                {
                    Player.GlobalUpdate();
                    PlayerBot.GlobalUpdatePosition();
                };

                updateTimer.Start();


            });
            // Heartbeat code here:

            Heartbeat.Init();

            // END Heartbeat code

            //PCCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //ProcessCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
            //Server.PCCounter.BeginInit();
            //ProcessCounter.BeginInit();
            //PCCounter.NextValue();
            //ProcessCounter.NextValue();
            
            //RENDERING DISABLED
            //renderThread = new Thread(new ThreadStart(IsoRender.RenderLevels));
            //renderThread.Start();


            ml.Queue(delegate
            {
                messageTimer.Elapsed += delegate
                {
                    RandomMessage();
                }; 
                messageTimer.Start();
                process = System.Diagnostics.Process.GetCurrentProcess();

                if (File.Exists("messages.txt"))
                {
                    StreamReader r = File.OpenText("messages.txt");
                    while (!r.EndOfStream)
                        messages.Add(r.ReadLine());
                }
                else
                    File.Create("messages.txt").Close();

                if (Server.irc)
                    new IRCBot();

                new AutoSaver(Server.backupInterval);     //2 and a half mins
                //Thread physThread = new Thread(new ThreadStart(Physics));
                //physThread.Start();
                /*if (Server.console)
                {
                    inputThread = new Thread(new ThreadStart(ParseInput));
                    inputThread.Start();
                }*/
            });
        }

        static bool Setup()
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Server.port);
                listen = new Socket(endpoint.Address.AddressFamily,
                                    SocketType.Stream, ProtocolType.Tcp);
                listen.Bind(endpoint);
                listen.Listen((int)SocketOptionName.MaxConnections);
                
                listen.BeginAccept(new AsyncCallback(Accept), null);
				return true;
            }
            catch (SocketException e) { ErrorLog(e); return false; }
            catch (Exception e) { ErrorLog(e); return false; }
        }

        static void Accept(IAsyncResult result)
        {
			// found information: http://www.codeguru.com/csharp/csharp/cs_network/sockets/article.php/c7695
			// -Descention
			try
			{
				new Player(listen.EndAccept(result));
				s.Log("New Connection");
				listen.BeginAccept(new AsyncCallback(Accept), null);
			}
			catch (SocketException e)
			{
				//s.Close();
				ErrorLog(e);
			}
			catch (Exception e)
			{
				//s.Close(); 
				ErrorLog(e);
			}
        }

        public static void Exit()
        {
            Player.players.ForEach(delegate(Player p) { p.Kick("Server shutdown."); });
            Player.connections.ForEach(delegate(Player p) { p.Kick("Server shutdown."); });

            Logger.Dispose();

            /*if (renderThread != null)
                renderThread.Abort();*/
            if (process != null)
            {
                ErrorLog("Process Terminated");
                process.Kill();
            }
            
        }

        public void PlayerListUpdate()
        {
            if (Server.s.OnPlayerListChange != null) Server.s.OnPlayerListChange(Player.players);
        }

        public void FailBeat()
        {
            if (HeartBeatFail != null) HeartBeatFail();
        }

        public void UpdateUrl(string url)
        {
            if (OnURLChange != null) OnURLChange(url);
        }

        public void Log(string message)
        {
            if (OnLog != null) OnLog(DateTime.Now.ToString("(HH:mm:ss) ") + message);
            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
        }
        public static void ErrorLog(string message)
        {
            if (Server.errlog == "") { Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "ERROR!"); }
            else
            {
                Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "ERROR! See \"" + Server.errlog + "\" for more information.");
                StreamWriter sw = File.AppendText(Server.errlog);
                sw.WriteLine(DateTime.Now.ToString("(HH:mm:ss)"));
                sw.WriteLine(message); sw.Close();
            }
        }

        public static void ErrorLog(Exception ex)
        {
            Logger.WriteError(ex);
        }

        public static void ParseInput()        //Handle console commands
        {
            string cmd;
            string msg;
            while (true)
            {
                string input = Console.ReadLine();
				if(input == null)
					continue;
                cmd = input.Split(' ')[0];
                if (input.Split(' ').Length > 1)
                    msg = input.Substring(input.IndexOf(' ')).Trim();
                else
                    msg = "";
                try
                {
                    switch (cmd)
                    {
                        case "rasm":
                            Command.all["reloadasm"].Use(null, msg); break;
                        case "kick":
                            Command.all["kick"].Use(null, msg); break;
                        case "ban":
                            Command.all["ban"].Use(null, msg); break;
                        case "banip":
                            Command.all["banip"].Use(null, msg); break;
                        case "resetbot":
                            Command.all["resetbot"].Use(null, msg); break;
                        case "save":
                            Command.all["save"].Use(null, msg); break;
                        case "say":
                            if (!msg.Equals(""))
                            {
                                if (Properties.ValidString(msg, "![]&:.,{}~-+()?_/\\@%$ "))
                                {
                                    Player.GlobalMessage(msg);
                                }
                                else
                                {
                                    Console.WriteLine("bad char in say");
                                }
                            }
                            break;
                        /*case "guest":
                            Command.all["guest"].Use(null, msg); break;
                        case "builder":
                            Command.all["builder").Use(null, msg); break;*/
						case "help":
                            Console.WriteLine("ban, banip, kick, resetbot, save, say");
							break;
                        default:
                            Console.WriteLine("No such command!"); break;
                    }
                }
                catch (Exception e) { ErrorLog(e); }
				//Thread.Sleep(10);
            }
        }
        
        public static void Physics()
        {
            int wait = 250;
            while (true)
            {
                try
                {
                    if (wait > 0)
                    {
                        Thread.Sleep(wait);
                    }
                    DateTime Start = DateTime.Now;
                    levels.ForEach(delegate(Level L)    //update every level
                    {
                        L.CalcPhysics();
                    });
                    TimeSpan Took = DateTime.Now - Start;
                    wait = (int)250 - (int)Took.TotalMilliseconds;
                    if (wait < -Server.Overload)
                    {
                        levels.ForEach(delegate(Level L)    //update every level
                        {
                            try
                            {
                                L.physics = 0;
                                L.ClearPhysics();
                            }
                            catch
                            {

                            }
                        });
                        Server.s.Log("!PHYSICS SHUTDOWN!");
                        Player.GlobalMessage("!PHYSICS SHUTDOWN!");
                        wait = 250;
                    }
                    else if (wait < (int)(-Server.Overload*0.75f))
                    {
                        Server.s.Log("!PHYSICS WARNING!");
                    }
                }
                catch
                {
                    Server.s.Log("GAH! PHYSICS EXPLODING!");
                    wait = 250;
                }
                
            }
		}

		public static void RandomMessage()
        {
            if (Player.number != 0 && messages.Count > 0)
                Player.GlobalMessage(messages[new Random().Next(0, messages.Count)]);
        }

        internal void SettingsUpdate()
        {
            if (OnSettingsUpdate != null) OnSettingsUpdate();
        }
    }
}