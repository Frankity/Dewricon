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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = MainForm.title;
            loadSettings();
        }

        public void loadSettings() {
            if (Voip == "True")
            { checkBox1.Checked = true; }
            else { checkBox1.Checked = false; }
            if (Sprint == 1)
                { checkBox2.Checked = true; }
            else { checkBox2.Checked = false; }
            numericUpDown1.Value = Mplayers;
        }

        public static string Voip;
        public static int Sprint;
        public static int Mplayers;

            public void GetData(string voip, int sprint, int mplayers)
            {
                Voip = voip;
                Sprint = sprint;
                Mplayers = mplayers;
            }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm d1 = new MainForm();
            var sname = textBox1.Text.ToString();
            d1.saveSettings(textBox1.Text, Voip , Sprint, Mplayers);
        }
    }
}
