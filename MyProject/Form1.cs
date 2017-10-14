using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using recognition;
using Emgu.CV;

namespace MyProject
{
    public partial class Form1 : Form
    {
        private Image img;
        private Capture capture = null;
        private int camNumber = 0;
        delegate void VoidDelegate();
        private Thread cameraThread = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //EXIFInform.EXIFInfo.DoIt(ofd.FileName);
                img = Image.FromFile(ofd.FileName);
                pictureBox1.Image = img;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Recognition RC = new Recognition(img);
            pictureBox3.Image = RC.GetRactang();
            pictureBox4.Image = RC.GetLines();
            pictureBox2.Image = RC.GetTemp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            img = pictureBox5.Image;
            pictureBox1.Image = img;
            button2.Enabled = true;
        }
        
        private void UpdateCameraInput()
        {
            while (true)
            {
                Thread.Sleep(50);
                if (capture == null)
                    capture = new Capture(camNumber);
                Mat mat = capture.QueryFrame();
                while (mat.Size.IsEmpty)
                    mat = capture.QueryFrame();
                Image<Emgu.CV.Structure.Bgr, Byte> img = mat.ToImage<Emgu.CV.Structure.Bgr, Byte>();
                int height = img.Height / 2;
                int width = height / 2;
                img.Draw(new Rectangle(img.Width / 2 - width / 2, height / 2, width, height), new Emgu.CV.Structure.Bgr(Color.Red));
                var bmp = img.ToBitmap();
                if (InvokeRequired)
                    pictureBox5.Invoke(new VoidDelegate(() => { UpdateCameraBox(bmp); }));
            }
        }

        private void UpdateCameraBox(Bitmap bitmap)
        {
            pictureBox5.Image = bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            camNumber = Int32.Parse(comboBox1.SelectedItem.ToString());
            capture = new Capture(camNumber);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            while ((new Capture(i)).QueryFrame() != null)
                i++;
            for(int j=0; j<i; j++)
            {
                comboBox1.Items.Add(j);
            }
            if(i != 0)
            {
                cameraThread = new Thread(UpdateCameraInput);
                cameraThread.Start();
                comboBox1.SelectedIndex = 0;
                button3.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            cameraThread?.Abort();
        }
    }
}
