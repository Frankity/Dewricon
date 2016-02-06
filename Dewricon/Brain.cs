using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Dewricon
{


    public class Brain
    {
        public static ConForm cc = new ConForm();
        

        public static string IP {get; set;}

        public WebSocket ws = new WebSocket("ws://" + cc.IP + ":" + "11776", "dew-rcon");
        public string lastMessage = "";
        public string lastCommand = "";

        public void Send(string command)
        {
            ws.Send(command);
            lastCommand = command;
        }
    }
}

