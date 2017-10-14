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

        private Image<Hsv, Byte> img;
        private Image<Hsv, Byte> temp;


        private byte[] Color = new byte[3];         //Centrinio pixelio spalva  

        //Koreguota pikelio spalva, naudojama ifui, siandien per paskaita suzinojau apie HVS, manau reikes perdaryt
        private int[] CPlus = new int[3];
        private int[] CMinus = new int[3];

        //Kokia paklaida nuo centrinio pixelio spalvos užskaityt pixelius kaip tinkamus
        private int Alpha;

        private int xv = 0, yv = 0; // virsutinis taskas
        private int xa = 0, ya = 0; // apatinis taskas
        private int xvd = 0, yvd = 200; // virsus desine
        private int xad = 0, yad = 200; // apacia desine
        private int xvk = 0, yvk = 200; // virsus kaire
        private int xak = 0, yak = 200; // apacia kaire

        public Recognition(Image X)
        {
            Recognize(X, 60);
        }

        //Nustatomos tinkamos spalvos paklaidos
        private void SetColourBounds(byte[] Color)
        {
            for (int i = 0; i < 3; i++)
            {
                CPlus[i] = Color[i] + Alpha;
                if (CPlus[i] > 255) CPlus[i] = 255;
                CMinus[i] = Color[i] - Alpha;
                if (CMinus[i] < 0) CMinus[i] = 0;
            }
        }

        private void Draw(Image<Hsv, Byte> X)
        {
            X.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yvd, xvd)), new Hsv(0, 0, 0), 1);
            X.Draw(new LineSegment2D(new Point(yvk, xvk), new Point(yak, xak)), new Hsv(0, 0, 0), 1);
            X.Draw(new LineSegment2D(new Point(yak, xak), new Point(ya, xa)), new Hsv(0, 0, 0), 1);
            X.Draw(new LineSegment2D(new Point(ya, xa), new Point(yad, xad)), new Hsv(0, 0, 0), 1);
            X.Draw(new LineSegment2D(new Point(yad, xad), new Point(yvd, xvd)), new Hsv(0, 0, 0), 1);
        }

        //Funkcija spalvinanti tinkancius pixelius
        private void DrawBeer(Image<Hsv, Byte> X, int x, int y)
        {
            for (int i = 0; i < 3; i++)
            {
                X.Data[x, y, i] = Color[i];
            }
        }

        //Randamas alaus viršus
        private void BeerTop(Image<Hsv, Byte> X)
        {
            for (int x = 200; x > 0; x--)
            {
                if ((X.Data[x, 200, 0] <= CPlus[0] && X.Data[x, 200, 0] >= CMinus[0]) &&
                    (X.Data[x, 200, 1] <= CPlus[1] && X.Data[x, 200, 1] >= CMinus[1]) &&
                    (X.Data[x, 200, 2] <= CPlus[2] && X.Data[x, 200, 2] >= CMinus[2]))
                {
                    DrawBeer(temp, x, 200);
                }
                else
                {
                    xv = x;
                    yv = 200;
                    break;
                }
            }
        }

        //Randama alaus apacia
        private void BeerBottom(Image<Hsv, Byte> X)
        {
            for (int x = 200; x < 400; x++)
            {
                if ((X.Data[x, 200, 0] <= CPlus[0] && X.Data[x, 200, 0] >= CMinus[0]) &&
                    (X.Data[x, 200, 1] <= CPlus[1] && X.Data[x, 200, 1] >= CMinus[1]) &&
                    (X.Data[x, 200, 2] <= CPlus[2] && X.Data[x, 200, 2] >= CMinus[2]))
                {
                    DrawBeer(temp, x, 200);
                }
                else
                {
                    xa = x;
                    ya = 200;
                    break;
                }
            }
        }

        //Virsutinis kairys taskas
        private void BeerUpperLeftPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(temp, x, y);
                    }
                    else
                    {
                        xvk = xv;
                        if (y < yvk) yvk = y;
                        break;
                    }
                }
            }
        }

        //Apatainis kairys taskas
        private void BeerLowerLeftPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(temp, x, y);
                    }
                    else
                    {
                        if (x > xak) xak = x;
                        if (y < yak) yak = y;
                        break;
                    }
                }
            }
        }

        //Virsutinis desinys taskas
        private void BeerUpperRightPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
            {
                for (int y = 200; y < 400; y++)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(temp, x, y);
                    }
                    else
                    {
                        xvd = xv;
                        if (y > yvd) yvd = y;
                        break;
                    }
                }
            }
        }

        //Apatinis desinys taskas
        private void BeerLowerRightPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y < 400; y++)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(temp, x, y);
                    }
                    else
                    {
                        if (x > xad) xad = x;
                        if (y > yad) yad = y;
                        break;
                    }
                }
            }
        }

        private void Recognize(Image X, int a)
        {
            //Nustatoma paklaida
            Alpha = a;

            //Sukuriami paveiksleliai ir paimama centrinio pixelio spalva
            img = new Image<Hsv, byte>(400, 400);
            temp = new Image<Hsv, byte>(400, 400);
            temp.SetValue(new Hsv(0, 0, 255));

            img.Bitmap = (Bitmap)X;
            img = img.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            //Pakoreguojamas paveiksliukas, kad geriau veiktu atpazinimo algoritmas
            img = img.SmoothBlur(7, 7);
            img = img.Dilate(3);
            img = img.Erode(5);

            //Gaunama centrinio pixelio spalva HSV koduoteje
            for (int i = 0; i < 3; i++)
            {
                Color[i] = img.Data[200, 200, i];
            }

            SetColourBounds(Color);

            //Kitas budas atpazinimo, su kuriuo dar nezinau ka daryti, juodai baltas paveikslikas
            Hsv Color1 = new Hsv(img.Data[200, 200, 0] - 60, img.Data[200, 200, 1] - 60, img.Data[200, 200, 2] - 60);
            Hsv Color2 = new Hsv(img.Data[200, 200, 0] + 60, img.Data[200, 200, 1] + 60, img.Data[200, 200, 2] + 60);
            Image<Gray, byte> hsvimg = new Image<Gray, byte>(400, 400);
            hsvimg = img.InRange(Color1, Color2);

            //Randami visi taškai ir piešiama kas matoma
            BeerTop(img);
            BeerBottom(img);
            BeerUpperLeftPoint(img);
            BeerLowerLeftPoint(img);
            BeerUpperRightPoint(img);
            BeerLowerRightPoint(img);

            //Nupiesiami paveiksliukai ir linijos ant jų
            Draw(temp);
            Draw(img);

            //Paveiksliukai konvertuojami atgal i Image tipa is emgucv Image tipo
            Lines = img.ToBitmap();
            Temp = temp.Bitmap;
            Ractang = hsvimg.Bitmap;

            //Atlaisvinama atmintis
            img.Dispose();
            temp.Dispose();
            hsvimg.Dispose();
        }
    }
}
