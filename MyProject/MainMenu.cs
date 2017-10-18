using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SocialTap
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void NextForm_Click(object sender, EventArgs e)
        {
            Regex valid = new Regex("[a-zA-Z]");
            if (!valid.IsMatch(BarName.Text))
            {
                MessageBox.Show("Bar Name is invadil. Use only Latin letters.");
            }
            else
            {
                Form1 m = new Form1();
                m.Show();
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
