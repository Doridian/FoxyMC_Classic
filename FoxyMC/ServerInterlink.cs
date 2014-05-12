using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using System.Threading;

namespace FoxyMC
{
    static class ServerInterlink
    {
        static Thread eventHandlerThread;
        static IrcClient client;

        static readonly string channel = "#dorifur-internal-mc";
        static readonly string nickprefix = "MCLink_";
        static readonly string nick = nickprefix + "Classic";
        static readonly int nickprefixlen = nickprefix.Length;

        public static void Start()
        {
            Stop();

            client = new IrcClient();

            eventHandlerThread = new Thread(new ThreadStart(client.Listen));

            client.OnConnected += new EventHandler(client_OnConnected);
            client.OnChannelMessage += new IrcEventHandler(client_OnChannelMessage);

            try
            {
                client.Connect("irc.doridian.de", 6666);
            }
            catch { }
        }

        static void client_OnChannelMessage(object sender, IrcEventArgs e)
        {
            IrcMessageData msg = e.Data;
            string name = msg.From.Substring(0, nickprefixlen);
            if (name != nickprefix) return;
            name = msg.From.Substring(nickprefixlen);
            string txt = msg.Message.Substring(1);
            switch (msg.Message[0])
            {
                case 'J':
                    SendServerMessage(name, "&a+ " + txt + "&e joined!");
                    break;
                case 'L':
                    SendServerMessage(name, "&c- " + txt + "&e left!");
                    break;
                case 'C':
                    SendServerMessage(name, txt);
                    break;
            }
        }

        static void SendServerMessage(string name, string msg)
        {
            Player.GlobalChat("&5[" + name + "] " + msg.Replace('§','&'));
        }

        static void client_OnConnected(object sender, EventArgs e)
        {
            eventHandlerThread.Start();
            client.Login(nick, nick, 0, nick);
            client.RfcJoin(channel, "dorifur3338");
        }

        public static void PlayerJoined(Player ply)
        {
            PlayerPack('J', ply);
        }

        public static void PlayerLeft(Player ply)
        {
            PlayerPack('L', ply);
        }

        public static void PlayerChat(Player ply, string chat)
        {
            PlayerPack('C', ply, ":&f " + chat);
        }

        static void PlayerPack(char id, Player ply)
        {
            PlayerPack(id, ply, "");
        }

        static void PlayerPack(char id, Player ply, string add)
        {
            SendPack(id, ply.color + ply.name + add);
        }

        static void SendPack(char id, string msg)
        {
            client.SendMessage(SendType.Message, channel, id + msg);
        }

        public static void Stop()
        {
            try
            {
                client.Disconnect();
            }
            catch { }
            try
            {
                eventHandlerThread.Abort();
            }
            catch { }
        }
    }
}
