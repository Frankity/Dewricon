using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WebSocketSharp;
using WebSocketSharp.Net;
using System.IO;

namespace Dewricon
{
    public class Brain
    {
        public static string path = AppDomain.CurrentDomain.BaseDirectory + @"Dewricon.txt";

        public static string[] List = File.ReadAllLines(path);

        public static string IP = List[1];
        public static string PORT = List[2];
        public static string PROTOCOL = List[3];

        public WebSocket ws = new WebSocket("ws://" + IP + ":" + PORT, PROTOCOL);
        public string lastMessage = "";
        public string lastCommand = "";

        public void Send(string command)
        {
            ws.Send(command);
            lastCommand = command;
        }
    }
}

