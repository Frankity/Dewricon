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
    public partial class PingP : DewPlugins.DewPlugins
    {
       
        public PingP() {
            ReadConfig(path);
        }

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

        static string path = AppDomain.CurrentDomain.BaseDirectory + @"plugins\PingPlugin.txt";

        public static string[] List = File.ReadAllLines(path);

        public static string IP = List[1];
        public static string PORT = List[2];
        public static string PROTOCOL = List[3];
        
        WebSocketSharp.WebSocket ws = new WebSocketSharp.WebSocket("ws://" + IP + ":" + PORT, PROTOCOL);

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
            string sgetjson = WCD.DownloadString("http://" + IP + ":11775/");
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
            //ws.OnMessage += ws_OnMessage;
            ws.Send("list");
            Console.WriteLine("sending list command");
            ws.Close();
            GetPlayers();
        }

        public void CheckFile()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("# Config file for PingPlugin ... do not delete this line.");
                tw.WriteLine("127.0.0.1");
                tw.WriteLine("11776");
                tw.WriteLine("dew-rcon");
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
                tw.WriteLine(pg.textBox2.Text);
                tw.WriteLine(pg.textBox3.Text);
                tw.WriteLine(pg.textBox4.Text);
                tw.WriteLine(pg.textBox1.Text);
                tw.WriteLine(pg.numericUpDown1.Value);
                tw.Close();
            }
        }

        public void ReadConfig(string path)
        {
            string[] lines = File.ReadAllLines(path);
            IP = lines[1];
            PORT = lines[2];
            PROTOCOL = lines[3];
            pg.textBox2.Text = lines[1];
            pg.textBox3.Text = lines[2];
            pg.textBox4.Text = lines[3];
            pg.textBox1.Text = lines[4];
            pg.numericUpDown1.Value = Int32.Parse(lines[5]);

        }

        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
            if (e.Data.Contains(""))
            {
            }
        }

    }
}

