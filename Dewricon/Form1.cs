using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using Dewricon.Helpers;

namespace Dewricon
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        //---------------------Plugins--------------------//

        public Dictionary<string, DewPlugins.DewPlugins> _plugins = new Dictionary<string, DewPlugins.DewPlugins>();

        public void LoadPlugins()
        {
            try
            {
                ICollection<DewPlugins.DewPlugins> plugins = GenLoadPlugin<DewPlugins.DewPlugins>.LoladPlugins(AppDomain.CurrentDomain.BaseDirectory + "plugins");
                foreach (var item in plugins)
                {
                    _plugins.Add(item.Name, item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //---------------------Plugins--------------------//

        }

        # region

        public static Brain dewRcons = new Brain();
        public bool dewRconConnected = false;
        public static string title = "";

        public void StartConnection()
        {
            try
            {
                //dewRcons.ws.Connect();
                dewRcons.ws.OnOpen += ws_OnOpen;
                dewRcons.ws.OnError += ws_OnError;
                dewRcons.ws.OnMessage += ws_OnMessage;
                connserver();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void saveSettings(string Nam, string V, int S, int Mp)
        {
            dewRcons.ws.Send("Server.Name " + Nam);
            dewRcons.ws.Send("Server.VoIP.Enabled " + V);
            dewRcons.ws.Send("Server.SprintEnabled " + S);
            dewRcons.ws.Send("Server.MaxPlayers " + Mp);
        }

        void ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            try
            {

                dewRcons.lastMessage = e.Data.ToString();
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText("[REC]: \n" + e.Data.ToString() + "\n")));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        void ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            dewRconConnected = false;
            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(e.Message + "\n")));
            StartConnection();
        }

        private void ws_OnOpen(object sender, EventArgs e)
        {
            dewRconConnected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dewRcons.Send(textBox3.Text);

        }

        private void btn_Send_to_console_Click(object sender, EventArgs e)
        {
            if (dewRcons.ws.IsAlive)
            {

                try
                {
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText("[SENT]: " + textBox3.Text + "\n\n")));
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
            else
            {
                MessageBox.Show("You're not conencted");
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
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText("[SENT]: " + textBox3.Text + "\n\n")));/*));*/
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
                    richTextBox1.Invoke(new Action(() => richTextBox1.AppendText("[SENT]: " + textBox3.Text + "\n\n")));/*));*/
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

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView1.Items[intselectedindex].Text;
                dewRcons.Send("Server.KickPlayer " + text);
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

        public static List<string> dick = new List<string>();

        Settings s22 = new Settings();

        public void connserver()
        {
            try
            {

                listView1.Items.Clear();

                System.Net.WebClient WCD = new System.Net.WebClient();
                string sgetjson = WCD.DownloadString("http://127.0.0.1:11775");
                dynamic getjson = JsonConvert.DeserializeObject(sgetjson);

                this.Text = "Dewricon: " + getjson["name"];
                title = getjson["name"];
                label4.Text = getjson["hostPlayer"];
                label6.Text = getjson["VoIP"];
                label8.Text = getjson["map"];
                label10.Text = getjson["variant"];
                label12.Text = getjson["numPlayers"];
                label14.Text = getjson["maxPlayers"];

                s22.GetData(getjson["VoIP"].ToString(), (int)getjson["sprintEnabled"], (int)getjson["maxPlayers"]);

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
                        dick.Add(name.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //    htp.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!dewRcons.ws.IsAlive)
            {
                dewRcons.ws.Close();
                StartConnection();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            connserver();
        }

        public class PluginsList
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string Version { get; set; }
            public string[] ToListViewItem()
            {
                return new string[] {
                    Name.ToString(),
                    Author.ToString(),
                    Version.ToString(),
                };
            }
        }

        List<PluginsList> items = new List<PluginsList>();
        public void PopulatePluginList()
        {
            foreach (var item in _plugins)
            {
                try
                {
                    items.Add(new PluginsList() { Name = item.Value.Name, Author = item.Value.Author, Version = item.Value.Version });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                for (int i = 0; i < items.Count; i++)
                {
                    ListViewItem v2 = new ListViewItem(items[i].ToListViewItem());
                    v2.Tag = items[i];
                    listView2.Invoke(new Action(() => listView2.Items.Add(v2)));
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            StartConnection();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            toolStripStatusLabel1.Text = "Vr: " + fvi.FileVersion;
            LoadPlugins();
            PopulatePluginList();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (dewRcons.ws.IsAlive)
            {
                dewRcons.ws.Close();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            connserver();
            Settings s1 = new Settings();
            s1.ShowDialog();
        }

        private void kickPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                dewRcons.ws.Send("Server.KickPlayer " + listView1.Items[intselectedindex].Text);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Thanks for using... Frankity");
        }

        # endregion

        private void listView2_ItemActivate(object sender, EventArgs e)
        {
            int intselectedindex = listView2.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView2.Items[intselectedindex].Text;
            }
            foreach (var item in _plugins)
            {
                if (items.Count == intselectedindex +1 )
                {
                    item.Value.Run();
                    
                }
            }
        }
    }
}




