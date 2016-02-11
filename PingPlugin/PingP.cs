using System;
using System.Linq;
using DewPlugins;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using System.Collections.Generic;


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
                return "0.0.2";
            }
        }
        public string Author
        {
            get
            {
                return "Frankity";
            }
        }

        public WebSocket ws = new WebSocket("ws://127.0.0.1:11776", "dew-rcon");

        public void Run()
        {
            ws.Connect();
            ws.OnMessage += ws_OnMessage;
            ws.Send("list");
            Console.WriteLine("sending list command");
            ws.Close();
        }

        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
        }

    }
}
