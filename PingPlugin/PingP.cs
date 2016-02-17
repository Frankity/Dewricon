using System;
using System.Linq;
using DewPlugins;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using WebSocketSharp.Net;
using WebSocketSharp;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace PingPlugin
{
    public class PingP : DewPlugins.DewPlugins
    {
        public string Name
        {
            get
            {
                return "Ping watcher plugin";
            }
        }
        public string Version
        {
            get
            {
                return "0.1.3";
            }
        }
        public string Author
        {
            get
            {
                return "Frankity";
            }
        }

        public static string PING { get; set; }
        public static int DELAY { get; set; }

        public WebSocket ws = new WebSocket("ws://127.0.0.1:11776", "dew-rcon");

        public static PluginForm pg = new PluginForm();

        public void Config()
        {
            CheckFile();
            pg.ShowDialog();
        }


        List<string> Players = new List<string>();

        public void GetPlayers()
        {
            System.Net.WebClient WCD = new System.Net.WebClient();
            string sgetjson = WCD.DownloadString("http://67.220.26.156:11775/");
            dynamic getjson = JsonConvert.DeserializeObject(sgetjson);

            var dew = getjson["players"];

            foreach (var item in dew)
            {
                try
                {
                    var name = item.name;
                     
                    Players.Add(name.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }
            derp();
        }

        public void derp()
        {
            for (int i = 0; i < Players.Count; i++)
			{
                ws.Send("ping " + Players[i]);
            }
        }

        public void Run()
        {
            ws.Connect();
            ws.OnMessage += ws_OnMessage;
            ws.Send("list");
            Console.WriteLine("sending list command");
            ws.Close();
            GetPlayers();
        }

        static string path = AppDomain.CurrentDomain.BaseDirectory + @"plugins\PingPlugin.txt";

        public void CheckFile()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("# Config file for PingPlugin ... do not delete this line.");
                tw.WriteLine("180");
                tw.WriteLine("7");
                tw.Close();
            }
            else
            {
                Console.WriteLine("the file already exist");
                ReadConfig(path);
            }
        }

        public void Stop()
        {
            Console.WriteLine("plugin Stoped");
        }

        public static void SaveConfig()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("# Config file for PingPlugin ... do not delete this line.");
                tw.WriteLine(pg.textBox1.Text);
                tw.WriteLine(pg.numericUpDown1.Value);
                tw.Close();
            }
        }

        public void ReadConfig(string path)
        {
            string[] lines = File.ReadAllLines(path);
            pg.textBox1.Text = lines[1];
            pg.numericUpDown1.Value = Int32.Parse(lines[2]);
        }

        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
        }

    }
}

