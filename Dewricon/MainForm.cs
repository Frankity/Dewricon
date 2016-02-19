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
using System.Threading;
using System.IO;

namespace Dewricon
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        //---------------------Plugins--------------------//

        public Dictionary<string, DewPlugins.DewPlugins> _plugins = new Dictionary<string, DewPlugins.DewPlugins>();

        public void LoadPlugins()
        {
            try
            {
                ICollection<DewPlugins.DewPlugins> plugins = GenLoadPlugin<DewPlugins.DewPlugins>.LoladPlugins(AppDomain.CurrentDomain.BaseDirectory + @"plugins\");
                foreach (var item in plugins)
                {
                    _plugins.Add(item.Author, item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

            PopulatePluginList();
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
                dewRcons.ws.OnOpen += ws_OnOpen;
                dewRcons.ws.OnError += ws_OnError;
                dewRcons.ws.OnMessage += ws_OnMessage;
                Thread t = new Thread(new ThreadStart(connserver));
                t.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Are you sure you are connected? check the settings tab");
                tabControl1.Invoke(new Action(() => tabControl1.SelectTab(3)));
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
                string sgetjson = WCD.DownloadString("http://" + Brain.IP + ":11775");
                dynamic getjson = JsonConvert.DeserializeObject(sgetjson);

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.Text = "Dewricon: " + getjson["name"]));
                }
                title = getjson["name"];
                if (label4.InvokeRequired)
                {
                    label4.Invoke(new Action(() => label4.Text = getjson["hostPlayer"]));
                }
                if (label6.InvokeRequired)
                {
                    label6.Invoke(new Action(() => label6.Text = getjson["VoIP"]));
                }
                if (label8.InvokeRequired)
                {
                    label8.Invoke(new Action(() => label8.Text = getjson["map"]));
                }
                if (label10.InvokeRequired)
                {
                    label10.Invoke(new Action(() => label10.Text = getjson["variant"]));
                }
                if (label12.InvokeRequired)
                {
                    label12.Invoke(new Action(() => label12.Text = getjson["numPlayers"]));
                }
                if (label14.InvokeRequired)
                {
                    label14.Invoke(new Action(() => label14.Text = getjson["maxPlayers"]));
                }

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
            catch (Exception)
            {
                MessageBox.Show("Are you sure you are connected? check the settings tab");
                tabControl1.Invoke(new Action(() => tabControl1.SelectTab(3)));
            }

            Thread.Sleep(2500);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
                dewRcons.ws.Close();
                StartConnection();
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
            public string Status { get; set; }
            public string[] ToListViewItem()
            {
                return new string[] {
                    Name.ToString(),
                    Author.ToString(),
                    Version.ToString(),
                    Status.ToString(),
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
                    items.Add(new PluginsList() { Name = item.Value.Name, Author = item.Value.Author, Version = item.Value.Version, Status = "No" });
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

        public void SaveConfig(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("#Do not delete this line... Set the server ip, port and protocol");
                tw.WriteLine(textBox1.Text);
                tw.WriteLine(textBox2.Text);
                tw.WriteLine(textBox4.Text);
                tw.Close();
            }
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            StartConnection();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            toolStripStatusLabel1.Text = "Vr: " + fvi.FileVersion;
            textBox1.Text = Brain.IP;
            textBox2.Text = Brain.PORT;
            textBox4.Text = Brain.PROTOCOL;
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

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView2.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView2.Items[intselectedindex].Text;
                listView2.Items[intselectedindex].SubItems[3].Text = "Yes";
            }
            foreach (var item in _plugins)
            {
                if (items.Count == intselectedindex + 1)
                {
                    item.Value.Run();
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView2.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView2.Items[intselectedindex].Text;
            }
            foreach (var item in _plugins)
            {
                if (items.Count == intselectedindex + 1)
                {
                    item.Value.Config();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intselectedindex = listView2.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                String text = listView2.Items[intselectedindex].Text;
                listView2.Items[intselectedindex].SubItems[3].Text = "No";
            }
            foreach (var item in _plugins)
            {
                if (items.Count == intselectedindex + 1)
                {
                    item.Value.Stop();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveConfig(Brain.path);
            DialogResult dr = MessageBox.Show("Config saved, to apply the changes you must restar the Tool\nDo you want to restart it?", "Atention", MessageBoxButtons.OKCancel);

            if (dr == DialogResult.OK)
            {
                System.Windows.Forms.Application.Restart();
                this.Close();
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            dewRcons.ws.Close();
        }

    }
}





