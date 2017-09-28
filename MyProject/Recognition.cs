using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;
using System.Drawing;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;
using Emgu.CV.Util;
using Emgu.CV.UI;
using System.Threading;

namespace recognition
{
    public class Recognition
    {
        //Paveiksliukai kurie yra redaguoti ir perduodami atgal, kad butu parodyti
        private Image Ractang;
        private Image Lines;
        private Image Temp;

        public Image GetRactang() { return Ractang; }
        public Image GetLines() { return Lines; }
        public Image GetTemp() { return Temp; }

        
        private byte[] Color = new byte[3];         //Centrinio pixelio spalva  
        //Koreguota pikelio spalva, naudojama ifui, siandien per paskaita suzinojau apie HVS, manau reikes perdaryt
        private int[] CPlus = new int[3];           
        private int[] CMinus = new int[3];

        //Kokia paklaida nuo centrinio pixelio spalvos užskaityt pixelius kaip tinkamus
        private int Alpha;

        public Recognition(Image X)
        {
            Recognize(X, 60);
        }

        //Nustatomos tinkamos spalvos
        public void SetColourBounds (byte[] Color)
        {
            for (int i = 0; i < 3; i++)
            {
                CPlus[i] = Color[i] + Alpha;
                if (CPlus[i] > 255) CPlus[i] = 255;
                CMinus[i] = Color[i] - Alpha;
                if (CMinus[i] < 0) CMinus[i] = 0;
            }
        }

        public void Recognize(Image X, int a)
        {
            //Nustatoma paklaida
            Alpha = a;

            //Sukuriami paveiksleliai ir paimama centrinio pixelio spalva
            Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 400);
            Image<Bgr, Byte> temp = new Image<Bgr, byte>(400, 400);
            temp.SetValue(new Bgr(255, 255, 255));

            img.Bitmap = (Bitmap)X;
            img.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            for (int i = 0; i < 3; i++)
            {
                Color[i] = img.Data[200, 200, i];
            }
            
            SetColourBounds(Color);
            
            int xv = 0, yv = 0; // virsutinis taskas
            int xa = 0, ya = 0; // apatinis taskas
            int xvd = 0, yvd = 200; // virsus desine
            int xad = 0, yad = 200; // apacia desine
            int xvk = 0, yvk = 200; // virsus kaire
            int xak = 0, yak = 200; // apacia kaire

            //Randamas alaus viršus
            for (int x = 200; x > 0; x--)
            {
                if ((img.Data[x, 200, 0] <= CPlus[0] && img.Data[x, 200, 0] >= CMinus[0]) &&
                    (img.Data[x, 200, 1] <= CPlus[1] && img.Data[x, 200, 1] >= CMinus[1]) &&
                    (img.Data[x, 200, 2] <= CPlus[2] && img.Data[x, 200, 2] >= CMinus[2]))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        temp.Data[x, 200, i] = Color[i];
                    }
                }
                else
                {
                    xv = x;
                    yv = 200;
                    break;
                }
            }

            //Randama alaus apacia
            for (int x = 200; x < 400; x++)
            {
                if ((img.Data[x, 200, 0] <= CPlus[0] && img.Data[x, 200, 0] >= CMinus[0]) &&
                    (img.Data[x, 200, 1] <= CPlus[1] && img.Data[x, 200, 1] >= CMinus[1]) &&
                    (img.Data[x, 200, 2] <= CPlus[2] && img.Data[x, 200, 2] >= CMinus[2]))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        temp.Data[x, 200, i] = Color[i];
                    }
                }
                else
                {
                    xa = x;
                    ya = 200;
                    break;
                }
            }            
            
            //Virsutinis kairys taskas
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((img.Data[x, y, 0] <= CPlus[0] && img.Data[x, y, 0] >= CMinus[0]) &&
                        (img.Data[x, y, 1] <= CPlus[1] && img.Data[x, y, 1] >= CMinus[1]) &&
                        (img.Data[x, y, 2] <= CPlus[2] && img.Data[x, y, 2] >= CMinus[2]))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            temp.Data[x, y, i] = Color[i];
                        }
                    }
                    else
                    {
                        xvk = xv;
                        if (y < yvk) yvk = y;
                        break;
                    }
                }
            }

            //Apatainis kairys taskas
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((img.Data[x, y, 0] <= CPlus[0] && img.Data[x, y, 0] >= CMinus[0]) &&
                        (img.Data[x, y, 1] <= CPlus[1] && img.Data[x, y, 1] >= CMinus[1]) &&
                        (img.Data[x, y, 2] <= CPlus[2] && img.Data[x, y, 2] >= CMinus[2]))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            temp.Data[x, y, i] = Color[i];
                        }
                    }
                    else
                    {
                        if (x > xak) xak = x;
                        if (y < yak) yak = y;
                        break;
                    }
                }
            }

            //Virsutinis desinys taskas
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
                {
                for (int y = 200; y < 400; y++)
                {
                    if ((img.Data[x, y, 0] <= CPlus[0] && img.Data[x, y, 0] >= CMinus[0]) &&
                        (img.Data[x, y, 1] <= CPlus[1] && img.Data[x, y, 1] >= CMinus[1]) &&
                        (img.Data[x, y, 2] <= CPlus[2] && img.Data[x, y, 2] >= CMinus[2]))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            temp.Data[x, y, i] = Color[i];
                        }
                    }
                    else
                    {
                        xvd = xv;
                        if (y > yvd) yvd = y;
                        break;
                    }
                }
            }
            
            //Apatinis desinys taskas
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y < 400; y++)
                {
                    if ((img.Data[x, y, 0] <= CPlus[0] && img.Data[x, y, 0] >= CMinus[0]) &&
                        (img.Data[x, y, 1] <= CPlus[1] && img.Data[x, y, 1] >= CMinus[1]) &&
                        (img.Data[x, y, 2] <= CPlus[2] && img.Data[x, y, 2] >= CMinus[2]))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            temp.Data[x, y, i] = Color[i];
                        }
                    }
                    else
                    {
                        if (x > xad) xad = x;
                        if (y > yad) yad = y;
                        break;
                    }
                }
            }

            //nupiesiamos mazos linijos ties taskais
            for (int i = 0; i < 3; i++)
            {
                for (int y = 0; y < 5; y++)
                {
                    temp.Data[xv + y, yv, i] = 0;
                    temp.Data[xa + y, ya, i] = 0;
                    temp.Data[xvk + y, yvk, i] = 0;
                    temp.Data[xvd + y, yvd, i] = 0;
                    temp.Data[xak + y, yak, i] = 0;
                    temp.Data[xad + y, yad, i] = 0;
                }
            }
            
            //Nupiesiami paveiksliukai
            temp.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yvd, xvd)), new Bgr(255, 0, 255), 1);
            temp.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yak, xak)), new Bgr(255, 0, 255), 1);
            temp.Draw(new LineSegment2D(new Point(yak, xak), new Point(ya, xa)), new Bgr(255, 0, 255), 1);
            temp.Draw(new LineSegment2D(new Point(ya, xa), new Point(yad, xad)), new Bgr(255, 0, 255), 1);
            temp.Draw(new LineSegment2D(new Point(yad, xad), new Point(yvd, xvd)), new Bgr(255, 0, 255), 1);

            img.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yvd, xvd)), new Bgr(255, 0, 255), 1);
            img.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yak, xak)), new Bgr(255, 0, 255), 1);
            img.Draw(new LineSegment2D(new Point(yak, xak), new Point(ya, xa)), new Bgr(255, 0, 255), 1);
            img.Draw(new LineSegment2D(new Point(ya, xa), new Point(yad, xad)), new Bgr(255, 0, 255), 1);
            img.Draw(new LineSegment2D(new Point(yad, xad), new Point(yvd, xvd)), new Bgr(255, 0, 255), 1);

            

            Lines = img.ToBitmap();
            Temp = temp.Bitmap;

            img.Dispose();
            temp.Dispose();
        }
    }
}
