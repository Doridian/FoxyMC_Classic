namespace FoxyMC.Commands
{
    using System;
    using System.Diagnostics;

    public class CmdServerReport : Command
    {
        public override void Help(Player p)
        {
            p.SendMessage("/serverreport - Get server CPU%, RAM usage, and uptime.");
        }

        public override void Use(Player p, string message)
        {
            TimeSpan totalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            TimeSpan span = (TimeSpan) (DateTime.Now - Process.GetCurrentProcess().StartTime);
            //string str = string.Concat(new object[] { "CPU Usage (Processes : All Processes):", Server.ProcessCounter.NextValue(), " : ", Server.PCCounter.NextValue() });
            string str2 = "Memory Usage: " + Math.Round((double) (((double) Process.GetCurrentProcess().PrivateMemorySize64) / 1048576.0)).ToString() + " Megabytes";
            string str3 = string.Concat(new object[] { "Uptime: ", span.Days, " Days ", span.Hours, " Hours ", span.Minutes, " Minutes ", span.Seconds, " Seconds" });
            p.SendMessage(str3);
            p.SendMessage(str2);
            //p.SendMessage(str);
        }

        public override string name
        {
            get
            {
                return "serverreport";
            }
        }

        public override LevelPermission level
        {
            get
            {
                return LevelPermission.Admin;
            }
        }
    }
}

