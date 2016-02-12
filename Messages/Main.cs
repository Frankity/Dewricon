using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DewPlugins;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Messages
{
    public class Messages : DewPlugins.DewPlugins
    {
        public string Name
        {
            get
            {
                return "Message Plugin";
            }
        }
        public string Version
        {
            get
            {
                return "0.0.3";
            }
        }
        public string Author
        {
            get
            {
                return "Frankity";
            }
        }

        public void ReadConfig()
        {
            StreamReader Sr = new StreamReader(@"\plugins\Messages.txt");
            Sr.ReadToEnd();

            Console.WriteLine(Sr.ToString());
        }

        public void Run()
        {
            ReadConfig(file);
            Console.WriteLine("Message Test :D");
        }
    }
}
