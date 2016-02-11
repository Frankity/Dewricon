﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Brain
{
    public class Brain
    {
        public WebSocket ws = new WebSocket("ws://127.0.0.1:11776", "dew-rcon");
        public string lastMessage = "";
        public string lastCommand = "";

        public void Send(string command)
        {
            ws.Send(command);
            lastCommand = command;
        }
    }
}

