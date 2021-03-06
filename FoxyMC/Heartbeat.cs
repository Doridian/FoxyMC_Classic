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
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace FoxyMC
{
	public class Heartbeat
	{
		static int _timeout = 60 * 1000;

		static string hash;
		public static string serverURL;
		static string staticVars;

		static BackgroundWorker worker;
		static HttpWebRequest request;

		public static void Init()
		{
			staticVars = "port=" + Server.port +
						 "&max=" + Server.players +
						 "&name=" + UrlEncode(Server.name) +
						 "&public=" + Server.pub +
						 "&version=" + Server.version;
			worker = new BackgroundWorker();
			worker.DoWork += new DoWorkEventHandler(worker_DoWork);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

			worker.RunWorkerAsync();
		}

		static void worker_DoWork(object sender, DoWorkEventArgs e)
		{
            Pump(Beat.Dori);
            Pump(Beat.Vanilla);
            //Console.Out.WriteLine("Heartbeat send!");
            Thread.Sleep(_timeout);
		}

		static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			worker.RunWorkerAsync();
		}
		/*
		Minecraft_Server.Heartbeat.Pump(Beat.Minecraft);
		Minecraft_Server.Heartbeat.Pump(Beat.fList);
		*/
		public static bool Pump(Beat type)
		{
			if (staticVars == null)
				Init();
			// default information to send
			string postVars = staticVars;

            string url = "http://minecraft.net/heartbeat.jsp";
			try
			{
				int hidden = 0;
				// append additional information as needed
				switch (type)
				{
                    case Beat.Dori:
                        url = "http://mc.foxba.se/heartbeat.jsp";
                        postVars += "&salt=" + Server.salt;
                        goto default;
                    case Beat.Vanilla:
						postVars += "&salt=" + Server.salt;
						goto default;
					default:
						postVars += "&users=" + (Player.number - hidden);
						break;

				}

				request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                //request.ServicePoint.BindIPEndPointDelegate = Server.BindIPEndPointCallback;
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
                request.Proxy = null;
				request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
				byte[] formData = Encoding.ASCII.GetBytes(postVars);
				request.ContentLength = formData.Length;
				request.Timeout = 15000;
				try
				{
					using (Stream requestStream = request.GetRequestStream())
					{
						requestStream.Write(formData, 0, formData.Length);
						requestStream.Close();
					}
				}
				catch (WebException e)
				{
					if (e.Status == WebExceptionStatus.Timeout)
					{

						throw new WebException("Failed during request.GetRequestStream()", e.InnerException, e.Status, e.Response);
					}
				}

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                    {
                        if (type == Beat.Vanilla)
                        {
                            string line = responseReader.ReadLine();

                            hash = line.Substring(line.LastIndexOf('/') + 1);
                            serverURL = line;

                            Server.s.UpdateUrl(serverURL);
                            File.WriteAllText("externalurl_vanilla.txt", serverURL);
                        }
                        else if (type == Beat.Dori)
                        {
                            string line = responseReader.ReadLine();

                            //hash = line.Substring(line.LastIndexOf('=') + 1);
                            serverURL = line;

                            Server.s.UpdateUrl(serverURL);
                            File.WriteAllText("externalurl_dori.txt", serverURL);
                        }
                    }
                }
                //Server.s.Log(string.Format("Heartbeat: {0}", type));
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.Timeout)
				{
                    Server.s.Log(string.Format("Timeout: {0}", type));
				}
				Server.ErrorLog(e);
			}
			catch (Exception e)
			{
                Server.s.Log(string.Format("Error reporting to {0}", type));
				Server.ErrorLog(e);
				return false;
			}
			finally
			{
				request.Abort();
			}
			return true;
		}

		public static string UrlEncode(string input)
		{
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if ((input[i] >= '0' && input[i] <= '9') ||
					(input[i] >= 'a' && input[i] <= 'z') ||
					(input[i] >= 'A' && input[i] <= 'Z') ||
					input[i] == '-' || input[i] == '_' || input[i] == '.' || input[i] == '~')
				{
					output.Append(input[i]);
				}
				else if (Array.IndexOf<char>(reservedChars, input[i]) != -1)
				{
					output.Append('%').Append(((int)input[i]).ToString("X"));
				}
			}
			return output.ToString();
		}
		public static char[] reservedChars = { ' ', '!', '*', '\'', '(', ')', ';', ':', '@', '&',
                                                 '=', '+', '$', ',', '/', '?', '%', '#', '[', ']' };
	}

	public enum Beat
	{
		Dori,
		Vanilla
	}
}
