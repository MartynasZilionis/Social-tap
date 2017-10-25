using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using recognition;
using Emgu.CV;
using DirectShowLib;


namespace SocialTap
{
    public partial class Form1 : Form
    {
        private Image img;
        private Image imgProcessing;
        private static Capture capture = null;
        private static Rectangle cameraRect;
        private static int lastHeight = 0;
        private static int lastWidth = 0;
        private Thread cameraThread = null;
        private int cameraIndex;
        private static readonly object imagesLock = new object();

        public Form1()
        {
            InitializeComponent();
            cameraRect = new Rectangle(0, 0, 1, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //EXIFInform.EXIFInfo.DoIt(ofd.FileName);
                imgProcessing?.Dispose();
                if (System.Text.RegularExpressions.Regex.IsMatch(ofd.FileName, ".jpg$"))
                {
                    imgProcessing = Image.FromFile(ofd.FileName);
                    pictureBox1.Image = imgProcessing;
                    button2.Enabled = true;
                }
                else MessageBox.Show("Netinkamo tipo failas");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Recognition RC = new Recognition(imgProcessing, 60);
            pictureBox3.Image = RC.HSV;
            pictureBox4.Image = RC.Lines;
            pictureBox2.Image = RC.Temp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            Thread thread = new Thread(button3_Click_ThreadSafe);
            thread.Start();
        }

        private delegate void Button3Delegate();
        private void button3_Click_ThreadSafe()
        {
            if (button3.InvokeRequired)
            {
                lock (imagesLock)
                {
                    try
                    {
                        Button3Delegate del = new Button3Delegate(button3_Click_ThreadSafe);
                        Invoke(del);
                    }
                    catch { }
                }
            }
            else
            {
                imgProcessing?.Dispose();
                Bitmap bmp = new Bitmap(img);
                imgProcessing = bmp.Clone(cameraRect, bmp.PixelFormat);
                pictureBox1.Image = imgProcessing;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
        
        private void UpdateCameraInput()
        {
            Mat mat = new Mat();
            Bitmap rawBitmap = new Bitmap(100,100);
            Image<Emgu.CV.Structure.Bgr, Byte> image = new Image<Emgu.CV.Structure.Bgr, byte>(rawBitmap);
            while (true)
            {
                Thread.Sleep(50);
                mat.Dispose();
                mat = new Mat();
                lock(imagesLock)
                {
                    //imagesMutex.WaitOne();
                    //isImagesMutexLocked = true;
                    if (capture == null)
                        continue;
                    //capture = new Capture(camNumber);
                    capture.Retrieve(mat);
                    if (mat == null)
                        continue;
                    while (mat.Size.IsEmpty)
                    {
                        capture.Retrieve(mat);
                    }
                    image.Dispose();
                    image = mat.ToImage<Emgu.CV.Structure.Bgr, Byte>();
                    rawBitmap.Dispose();
                    rawBitmap = image.ToBitmap();
                    UpdateCameraRect(image);
                    image.Draw(cameraRect, new Emgu.CV.Structure.Bgr(Color.Red));
                    UpdateCameraBox(image.Bitmap, rawBitmap);
                    //imagesMutex.ReleaseMutex();
                    //isImagesMutexLocked = false;
                }
            }
        }

        private delegate void UpdateCameraBoxDelegate(Bitmap processed, Bitmap raw);
        private void UpdateCameraBox(Bitmap processed, Bitmap raw)
        {
            if(pictureBox5.InvokeRequired)
            {
                try
                {
                    UpdateCameraBoxDelegate del = new UpdateCameraBoxDelegate(UpdateCameraBox);
                    Invoke(del, new object[] { processed, raw });
                }
                catch { }
            }
            else
            {
                img = raw;
                pictureBox5.Image = processed;
            }
        }

        private void UpdateCameraRect(Image<Emgu.CV.Structure.Bgr, byte> img)
        {
            if (img.Height == lastHeight && img.Width == lastWidth)
                return;
            int height = img.Height / 2;
            int width = height / 2;
            cameraRect = new Rectangle(img.Width / 2 - width / 2, height / 2, width, height);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == cameraIndex)
                return;
            capture?.Dispose();
            cameraIndex = comboBox1.SelectedIndex;
            capture = new Capture(cameraIndex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //DirectShow.NET
            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            var WebCams = new Video_Device[_SystemCamereas.Length];
            for (int i = 0; i < _SystemCamereas.Length; i++)
            {
                WebCams[i] = new Video_Device(i, _SystemCamereas[i].Name, _SystemCamereas[i].ClassID); //fill web cam array
                comboBox1.Items.Add(WebCams[i].ToString());
            }
            if (comboBox1.Items.Count > 0)
            {
                cameraIndex = -1;
                comboBox1.SelectedIndex = 0; //Select the default device
                button3.Enabled = true; //Enable photo button
                cameraThread = new Thread(UpdateCameraInput);
                cameraThread.Start();
            }
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            cameraThread?.Abort();
        }
    }
}
