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
    public partial class ConForm : Form
    {
        public ConForm()
        {
            InitializeComponent();
        }

        public string IP;

        private void button1_Click(object sender, EventArgs e)
        {
            IP = textBox1.Text;
            Form1 f1 = new Form1();
            f1.StartConnection(IP);
        }
    }
}
