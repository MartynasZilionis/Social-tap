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
        
        public void NextForm_Click(object sender, EventArgs e)
        {
            if (!RegexValidate(BarName.Text))
            {
                MessageBox.Show("Bar name is invadil. \nIt should be: \n 1. only one word \n 2. starting with uppercase \n 3. every other letter - lowercase \n 4. use only Lithuanian letters.");
            }
            else
            {
                Form1 m = new Form1();
                m.Show();
            }
        }

        public bool RegexValidate(string text)
        {
            Regex valid = new Regex("^[A-ZĄČĘĖĮŠŲŪŽ][a-ząčęėįšųūž]{1,10}$");
            return valid.IsMatch(text);
        }
    }
}
