using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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
            textBox3.Enabled = true;
            btn_Send_to_console.Enabled = true;
        }



        void ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            try
            {

                dewRcons.lastMessage = e.Data.ToString();
                richTextBox1.Invoke(new Action(() => richTextBox1.Text += "[REC]: \n" + e.Data.ToString() + "\n"));
                //Console.WriteLine(e.RawData.ToString());
                string omg;
                string qrep;
                string uid;

                string omfg = "[0] " + '"' + "SkarLeth" + '"' + " (uid: 5076eb6fd7397b60, ip: 94.62.249.171) [1] " + '"' + "LordGrayHam" + '"' + " (uid: b765e8d8cd05bceb, ip: 31.193.220.30) [2] " + '"' + "Devairen" + '"' + "(uid: d83832c7e14c0I3be, ip: 84.197.26.237) [3] " + '"' + "LikeABob (GER)" + '"' + "(uid: 3a7eef2a1b03ed75, ip: 84.131.156.201) [4] " + '"' + "Keyboiii" + '"' + "(uid: de941af4117ad449, ip: 86.6.62.52) [5] " + '"' + "Purity" + '"' + "(uid: ab9d366cc6c496ca, ip: 190.80.64.135) [6] " + '"' + "Maggie" + '"' + "(uid: bac8ffb5126c5a00, ip: 151.226.75.236) [7] " + '"' + "Sharky" + '"' + "(uid: b94d7ee8fTT75fab, ip: 84.211.123.106) [8] " + '"' + "suzzo" + '"' + "(uid: cef0fb72adea504b, ip: 79.22.46.99) [9] " + '"' + "Major Baked" + '"' + "(uid: aeada408cd90d7f6, ip: 80.7.81.136) [10] " + '"' + "NimeroKing" + '"' + "(uid: 7a4801771c0e7f1c, ip: 109.91.32.0) [11] " + '"' + "Pasta Batman" + '"' + "(uid: fb9e9dccb26ca12d, ip: 217.39.126.248) [12] " + '"' + "Rowsdower" + '"' + "(uid: 1c96f6ea2278a9d3, ip: 75.118.6.153) [14] " + '"' + "dontshoot" + '"' + "(uid: 99d27455be9faf8c, ip: 129.241.136.108) ";

                if (omfg.Contains("uid"))
                {
                    string[] thisarray = e.Data.Split(')');
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StartConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                dewRcons.Send("server.listplayers");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                dewRcons.ws.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
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
            catch (Exception)
            {

                throw;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            btn_Send_to_console.Enabled = false;
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dewRcons.Send("Server.KickUid " + listView1.Items[0].SubItems[2].Text);
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView1.Items[intselectedindex].Text;
                MessageBox.Show(listView1.Items[intselectedindex].Text);
            } 
        }

        public class Players
        {
            public string Name { get; set; }
            public string Score { get; set; }
            public string Kills { get; set; }
            public string Assists { get; set; }
            public string Deaths { get; set; }
            public string Uid { get; set; }
            public string[] ToListViewItem()
            {
                return new string[] {
                    Name.ToString(),
                    Score.ToString(),
                    Kills.ToString(),
                    Assists.ToString(),
                    Deaths.ToString(),
                    Uid.ToString()
                };
            }
        }

        // test code
        void Button4Click(object sender, EventArgs e)
        {


            listView1.Items.Clear();

            System.Net.WebClient WCD = new System.Net.WebClient();
            string sgetjson = WCD.DownloadString("http://65.188.39.87:11775");
            dynamic getjson = JsonConvert.DeserializeObject(sgetjson);

            this.Text = "Dewricon: " + getjson["name"];
            label4.Text = getjson["hostPlayer"];
            label6.Text = getjson["VoIP"];
            label8.Text = getjson["map"];
            label10.Text = getjson["variant"];
            label12.Text = getjson["numPlayers"];
            label14.Text = getjson["maxPlayers"];

            if (getjson["teams"] == 0)
            {
                
            }

            List<Players> items = new List<Players>();
            var dew = getjson["players"];

            foreach (var item in dew)
            {
                try
                {
                    var name = item.name;
                    var score = item.score;
                    var kills = item.kills;
                    var assists = item.assists;
                    var deaths = item.deaths;
                    var uid = item.uid;
                    items.Add(new Players() { Name = name, Score = score, Kills = kills, Assists = assists, Deaths = deaths, Uid = uid });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                ListViewItem v2 = new ListViewItem(items[i].ToListViewItem());
                v2.Tag = items[i];
                listView1.Invoke(new Action(() => listView1.Items.Add(v2)));
            }
        }


        private void kickByUidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                //String text = listView1.Items[intselectedindex].SubItems[5].Text;
                MessageBox.Show(listView1.Items[intselectedindex].SubItems[5].Text);
                //   dewRcons.Send("Server.KickUid " + listView1.Items[0].SubItems[2].Text);   
            } 
        }


    }
}




