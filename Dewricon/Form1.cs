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
            //  richTextBox1.Invoke(new Action(() => richTextBox1.Text += dewRcons.lastMessage.ToString() + "\n"));
            // richTextBox1.Invoke(new Action(() => richTextBox1.Text += dewRcons.lastMessage + "\n"));
            richTextBox1.Invoke(new Action(() => richTextBox1.Text += "[REC]: \n" + e.Data.ToString() + "\n"));
            //  richTextBox1.AppendText("[REC]:\n" + e.Data.ToString());
            Console.WriteLine(e.RawData.ToString());
            if (e.Data.Contains("uid"))
            {

                string[] thisarray = e.Data.Split(' ');
                List<string> Mylist = new List<string>();
                Mylist.AddRange(thisarray);
                foreach (var item in Mylist)
                {
                   // listView1.Invoke(new Action(() => listView1.Items.Add(item[1].ToString())));

                    string[] split = item.Split(',');
                    
                    if (split.Length > 0)
                    {
                        //ListViewItem lvi = new ListViewItem(split[i]);
                        for (int i = 0; i < split.Length; i++)
                        { 
                            if (split[i] == "ip:" || split[i].Contains("uid"))
                            {
                            }else{
                                string omg;
                                if (!split[i].Contains(')')) 
                                {
                                    listView1.Invoke(new Action(() => listView1.Items[0].SubItems.Add(split[1].ToString())));
                                }
                                else
                                {
                                    omg = split[i].Replace(')', ' ');
                                    listView1.Invoke(new Action(() => listView1.Items[i].SubItems.Add(omg.ToString())));
                                }

                            }
                           // lvi.SubItems.Add(split[i]);
                        }
                        
                    }
                    
                }
                dewRcons.ws.Close();

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



    }
}
