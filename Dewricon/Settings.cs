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
            textBox1.Text = Form1.title;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 d1 = new Form1();
            var sname = textBox1.Text.ToString();
            d1.saveSettings(sname);
        }
    }
}
