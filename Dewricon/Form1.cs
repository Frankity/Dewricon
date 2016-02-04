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
				
				string omfg = "[0] " + '"' + "SkarLeth" + '"' + " (uid: 5076eb6fd7397b60, ip: 94.62.249.171) [1] " + '"' + "LordGrayHam" + '"' + " (uid: b765e8d8cd05bceb, ip: 31.193.220.30) [2] " + '"' + "Devairen"  + '"' + "(uid: d83832c7e14c0I3be, ip: 84.197.26.237) [3] " + '"' + "LikeABob (GER)"  + '"' + "(uid: 3a7eef2a1b03ed75, ip: 84.131.156.201) [4] " + '"' + "Keyboiii" + '"' + "(uid: de941af4117ad449, ip: 86.6.62.52) [5] " + '"' + "Purity" + '"' + "(uid: ab9d366cc6c496ca, ip: 190.80.64.135) [6] " + '"' + "Maggie"  + '"' + "(uid: bac8ffb5126c5a00, ip: 151.226.75.236) [7] " + '"' + "Sharky"  + '"' + "(uid: b94d7ee8fTT75fab, ip: 84.211.123.106) [8] " + '"' + "suzzo" + '"' + "(uid: cef0fb72adea504b, ip: 79.22.46.99) [9] " + '"' + "Major Baked"  + '"' + "(uid: aeada408cd90d7f6, ip: 80.7.81.136) [10] " + '"' + "NimeroKing"  + '"' + "(uid: 7a4801771c0e7f1c, ip: 109.91.32.0) [11] " + '"' +"Pasta Batman"  + '"' +"(uid: fb9e9dccb26ca12d, ip: 217.39.126.248) [12] " + '"' +"Rowsdower" + '"' + "(uid: 1c96f6ea2278a9d3, ip: 75.118.6.153) [14] " + '"' +"dontshoot" + '"' + "(uid: 99d27455be9faf8c, ip: 129.241.136.108) ";
				
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
			dewRcons.Send("Server.KickUid " + listView1.Items[0].SubItems[2].Text);
		}
		
		// test code
		void Button4Click(object sender, EventArgs e)
		{
			
			string omfg = " [0] " + '"' + "SkarLeth" + '"' + " (uid: 5076eb6fd7397b60, ip: 94.62.249.171) [1] " + '"' + "LordGrayHam" + '"' + " (uid: b765e8d8cd05bceb, ip: 31.193.220.30) [2] " + '"' + "Devairen"  + '"' + " (uid: d83832c7e14c0I3be, ip: 84.197.26.237) [3] " + '"' + "LikeABob"  + '"' + " (uid: 3a7eef2a1b03ed75, ip: 84.131.156.201) [4] " + '"' + "Keyboiii" + '"' + " (uid: de941af4117ad449, ip: 86.6.62.52) [5] " + '"' + "Purity" + '"' + " (uid: ab9d366cc6c496ca, ip: 190.80.64.135) [6] " + '"' + "Maggie"  + '"' + " (uid: bac8ffb5126c5a00, ip: 151.226.75.236) [7] " + '"' + "Sharky"  + '"' + " (uid: b94d7ee8fTT75fab, ip: 84.211.123.106) [8] " + '"' + "suzzo" + '"' + " (uid: cef0fb72adea504b, ip: 79.22.46.99) [9] " + '"' + "Major Baked"  + '"' + " (uid: aeada408cd90d7f6, ip: 80.7.81.136) [10] " + '"' + "NimeroKing"  + '"' + " (uid: 7a4801771c0e7f1c, ip: 109.91.32.0) [11] " + '"' +"Pasta Batman"  + '"' +" (uid: fb9e9dccb26ca12d, ip: 217.39.126.248) [12] " + '"' +"Rowsdower" + '"' + " (uid: 1c96f6ea2278a9d3, ip: 75.118.6.153) [14] " + '"' +"dontshoot" + '"' + " (uid: 99d27455be9faf8c, ip: 129.241.136.108)".Replace('(','-');
			//string okdf = omfg.Replace(')','-');
			List<string> xD = new List<string>();
			if (omfg.Contains("uid"))
			{
				string[] thisarray = omfg.Split(')');
				List<string> Mylist = new List<string>();
				Mylist.AddRange(thisarray);
				for (int i = 0; i < Mylist.Count; i++) {
					xD.Add(Mylist[i].Replace(' ','*'));
					//Mylist[i].Remove(' ');
				}
				List<string> Slit = new List<string>();
				//  dewRcons.ws.Close();
				
				foreach (var element in xD) {
					string[] secarrat = element.Split('*');
					Slit.AddRange(secarrat);
				}
				for (int i = 0; i < Slit.Count; i++) {
					Slit.Remove("");
					Slit.Remove("ip:");
					Slit.Remove("(uid:");
				}
				
				
				foreach (var item in Slit)
				{
					var id = item;
					ListViewItem lvi = new ListViewItem(item);
					listView1.Invoke(new Action(() => listView1.Items.Add(id.ToString())));

					// conver to  json and work on it :D
							
				}
			}
		}
	}
 	
}


