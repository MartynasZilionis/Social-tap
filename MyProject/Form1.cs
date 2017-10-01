using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;

using recognition;

namespace MyProject
{
    public partial class Form1 : Form
    {
        private Image img;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                EXIFInform.EXIFInfo.DoIt(ofd.FileName);
                pictureBox1.ImageLocation = ofd.FileName;
                img = Image.FromFile(ofd.FileName);
                //Console.WriteLine(ofd.FileName);
                





            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Recognition RC = new Recognition(img);
            pictureBox3.Image = RC.GetRactang();
            pictureBox4.Image = RC.GetLines();
            pictureBox2.Image = RC.GetTemp();
        }
    }
}
