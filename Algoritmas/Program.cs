using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algoritmas
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Recognition();
        }

        static void Recognition()
        {
            StringBuilder msgBuilder = new StringBuilder("Performance: ");

            //Load the image from file and resize it for display
            Image<Bgr, Byte> img =
               new Image<Bgr, byte>("..\\..\\pvz.jpg")
               .Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            //-----------------------------------------------------------------------------------------------------------------------------------------
            //img._EqualizeHist();
            //img._GammaCorrect(1.0d);
            //-----------------------------------------------------------------------------------------------------------------------------------------

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

            //-----------------------------------------------------------------------------------------------------------------------------------------
            img.Save("F:\\Main\\Main\\C# social tap\\Algoritmas\\Algoritmas\\int.jpg");
            //-----------------------------------------------------------------------------------------------------------------------------------------
            Stopwatch watch = Stopwatch.StartNew();
            double cannyThreshold = 0;

            #region Canny and edge detection
            watch.Start();
            double cannyThresholdLinking = 120.0;
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);

            LineSegment2D[] lines = CvInvoke.HoughLinesP(
               cannyEdges,
               1, //Distance resolution in pixel-related units
               Math.PI / 45.0, //Angle resolution measured in radians.
               20, //threshold
               30, //min Line width
               10); //gap between lines

            watch.Stop();
            msgBuilder.Append(String.Format("Canny & Hough lines - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            #region Find triangles and rectangles
            watch.Reset(); watch.Start();

            List<Triangle2DF> triangleList = new List<Triangle2DF>();
            List<RotatedRect> boxList = new List<RotatedRect>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size == 4) //The contour has 4 vertices.
                            {
                                #region determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                       edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                #endregion

                                if (isRectangle)
                                {
                                    Console.WriteLine("Staciakampis");
                                    Console.ReadLine();
                                    boxList.Add(CvInvoke.MinAreaRect(approxContour));
                                }
                            }
                        }
                    }
                }
            }

            watch.Stop();
            msgBuilder.Append(String.Format("Triangles & Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            PictureBox triangleRectangleImageBox = new ImageBox();
            PictureBox lineImageBox = new ImageBox();            

            #region draw triangles and rectangles
            Image<Bgr, Byte> triangleRectangleImage = img.CopyBlank();

            foreach (RotatedRect box in boxList)
                triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
            triangleRectangleImageBox.Image = triangleRectangleImage.ToBitmap();
            #endregion

            #region draw lines
            Image<Bgr, Byte> lineImage = img.CopyBlank();
            foreach (LineSegment2D line in lines)
                lineImage.Draw(line, new Bgr(Color.Green), 2);
            lineImageBox.Image = lineImage.ToBitmap();
            #endregion

            triangleRectangleImageBox.Image.Save("F:\\Main\\Main\\C# social tap\\Algoritmas\\Algoritmas\\rez1.jpg");
            lineImageBox.Image.Save("F:\\Main\\Main\\C# social tap\\Algoritmas\\Algoritmas\\rez2.jpg");
            
        }
    }
}
