using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Dewricon
{
    public partial class HttpServer
    {
        private HttpListener _listener;
        private Thread _thread;

        public HttpServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:1434/");
        }

        public void Start()
        {
            _listener.Start();
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(1);

                try
                {
                    var context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(state =>
                        {
                            OnResponse((HttpListenerContext)state);
                        }, context);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }

        public void OnResponse(HttpListenerContext context)
        {
            var url = context.Request.Url.PathAndQuery;
            Console.WriteLine(string.Format("HTTP request form {0}", url));
            var urlparts = url.Substring(1).Split(new[] { '/' }, 2);
            if (urlparts.Length >= 1)
            {
                if (urlparts[0] == "players")
                {

                    Console.WriteLine("lol");

                    List<string> li = new List<string>();
                    foreach (var item in MainForm.dick)
                    {
                        li.Add(item);
                    }
                        for (int i = 0; i < MainForm.dick.Count; i++)
                        {
                            var derp = li[i];
                            Console.WriteLine(derp);
                            var b = Encoding.ASCII.GetBytes(derp.ToString());
                            context.Response.ContentLength64 = b.Length;
                            context.Response.ContentType = "text/plain";
                            context.Response.OutputStream.Write(b, 0, b.Length);
                            context.Response.OutputStream.Close();
                            return;
                        }
                }
            }
        }
    }
}
