using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dewricon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Brain dewRcons = new Brain();
        public bool dewRconConnected = false;

        public void StartConnection()
        {
            dewRcons.ws.Connect();
            //dewRcons.ws.ConnectAsync(); 
            dewRcons.ws.OnOpen += ws_OnOpen;
            dewRcons.ws.OnError += ws_OnError;
            dewRcons.ws.OnMessage += ws_OnMessage;
        }



        void ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            dewRcons.lastMessage = e.Data.ToString();
            richTextBox1.Invoke(new Action(() => richTextBox1.Text += "[REC]: \n" + e.Data.ToString() + "\n"));
            Console.WriteLine(e.RawData.ToString());
            string omg;
            string qrep;
            string uid;
            if (e.Data.Contains("uid"))
            {
                string[] thisarray = e.Data.Split(' ');
                List<string> Mylist = new List<string>();
                Mylist.AddRange(thisarray);
                foreach (var item in Mylist)
                {
                    if (item == "ip:" || item.Contains("uid"))
                    {
                    }
                    else if (item.Contains('"'))
                    {
                        qrep = item.Replace('"', ' ');
                        listView1.Invoke(new Action(() => listView1.Items[0].SubItems.Add(qrep.ToString())));
                    }
                    else if (item.Length > 15 && item.Length < 18)
                    {
                        uid = item.Replace(',', ' ');
                        listView1.Invoke(new Action(() => listView1.Items[0].SubItems.Add(uid.ToString())));
                    }
                    else
                    {
                        if (!item.Contains(')'))
                        {
                            listView1.Invoke(new Action(() => listView1.Items.Add(item.ToString())));
                        }
                        else
                        {
                            omg = item.Replace(')', ' ');
                            listView1.Invoke(new Action(() => listView1.Items[0].SubItems.Add(omg.ToString())));
                        }
                    }
                }
                //  dewRcons.ws.Close();

            }
        }

        void ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            dewRconConnected = false;
            richTextBox1.Invoke(new Action(() => richTextBox1.Text += e.Message + "\n"));
            StartConnection();
        }

        private void ws_OnOpen(object sender, EventArgs e)
        {
            dewRconConnected = true;
            //   MessageBox.Show("Connected");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dewRcons.Send(textBox3.Text);

        }


        private void btn_Send_to_console_Click(object sender, EventArgs e)
        {
            //richTextBox1.AppendText("[SENT]:\n" + textBox3.Text);
            richTextBox1.Invoke(new Action(() => richTextBox1.Text += "[SENT]: " + textBox3.Text + "\n\n"));/*));*/
            if (textBox3.Text.StartsWith("/clear"))
            {
                richTextBox1.Clear();
                textBox3.Clear();
            }
            else
            {
                dewRcons.Send(textBox3.Text);
                textBox3.Clear();

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StartConnection();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dewRcons.Send("server.listplayers");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dewRcons.ws.Close();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.Text += "[SENT]: " + textBox3.Text + "\n\n"));/*));*/
                if (textBox3.Text.StartsWith("/clear"))
                {
                    richTextBox1.Clear();
                    textBox3.Clear();
                }
                else
                {
                    dewRcons.Send(textBox3.Text);
                    textBox3.Clear();
                }
            }
        }

    }
}
