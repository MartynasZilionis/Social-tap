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
        public Image HSV   { get; private set; }
        public Image Lines { get; private set; }
        public Image Temp  { get; private set; }

        //Laikini paveiksliukai, palengvinantys testavima, 0 - ka randa programa, 1 - kaip perdaromas paveiksliukas
        private List<Image<Hsv, Byte>> Laikini = new List<Image<Hsv, Byte>>();

        private byte[] Color = Enumerable.Repeat<byte>(0, 3).ToArray();         //Centrinio pixelio spalva  

        //Koreguota pikelio spalva, naudojama ifui, siandien per paskaita suzinojau apie HVS, manau reikes perdaryt
        private int[] CPlus = Enumerable.Repeat(0, 3).ToArray();
        private int[] CMinus = Enumerable.Repeat(0, 3).ToArray();

        //Kokia paklaida nuo centrinio pixelio spalvos užskaityt pixelius kaip tinkamus
        private int Alpha;

        public int xv = 0, yv = 0; // virsutinis taskas
        public int xa = 0, ya = 0; // apatinis taskas
        public int xvd = 0, yvd = 200; // virsus desine
        public int xad = 0, yad = 200; // apacia desine
        public int xvk = 0, yvk = 200; // virsus kaire
        public int xak = 0, yak = 200; // apacia kaire

        //-----------------------------------------------------------
        // UNIT TESTAMAS TIK
        public Recognition() { }
        public void ADD_TO_LIST()
        {
            Laikini.Add(new Image<Hsv, byte>(400, 400));
            Laikini.Add(new Image<Hsv, byte>(400, 400));
        }
        //------------------------------------------------------------

        // Toliau kas reikia
        public Recognition(Image X, int alpha = 60)
        {
            Recognize(skirtumas: alpha, pav: X);
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
        public void BeerTop(Image<Hsv, Byte> X)
        {
            for (int x = 200; x > 0; x--)
            {
                if ((X.Data[x, 200, 0] <= CPlus[0] && X.Data[x, 200, 0] >= CMinus[0]) &&
                    (X.Data[x, 200, 1] <= CPlus[1] && X.Data[x, 200, 1] >= CMinus[1]) &&
                    (X.Data[x, 200, 2] <= CPlus[2] && X.Data[x, 200, 2] >= CMinus[2]))
                {
                    DrawBeer(Laikini[1], x, 200);
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
        public void BeerBottom(Image<Hsv, Byte> X)
        {
            for (int x = 200; x < 400; x++)
            {
                if ((X.Data[x, 200, 0] <= CPlus[0] && X.Data[x, 200, 0] >= CMinus[0]) &&
                    (X.Data[x, 200, 1] <= CPlus[1] && X.Data[x, 200, 1] >= CMinus[1]) &&
                    (X.Data[x, 200, 2] <= CPlus[2] && X.Data[x, 200, 2] >= CMinus[2]))
                {
                    DrawBeer(Laikini[1], x, 200);
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
        public void BeerUpperLeftPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(Laikini[1], x, y);
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
        public void BeerLowerLeftPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y > 0; y--)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(Laikini[1], x, y);
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
        public void BeerUpperRightPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv; x < xv + ((xa - xv) / 2); x++)
            {
                for (int y = 200; y < 400; y++)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(Laikini[1], x, y);
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
        public void BeerLowerRightPoint(Image<Hsv, Byte> X)
        {
            for (int x = xv + ((xa - xv) / 2); x < xa; x++)
            {
                for (int y = 200; y < 400; y++)
                {
                    if ((X.Data[x, y, 0] <= CPlus[0] && X.Data[x, y, 0] >= CMinus[0]) &&
                        (X.Data[x, y, 1] <= CPlus[1] && X.Data[x, y, 1] >= CMinus[1]) &&
                        (X.Data[x, y, 2] <= CPlus[2] && X.Data[x, y, 2] >= CMinus[2]))
                    {
                        DrawBeer(Laikini[1], x, y);
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

        private void Recognize(Image pav, int skirtumas)
        {
            //Nustatoma paklaida
            Alpha = skirtumas;

            Laikini.Add(new Image<Hsv, byte>(400, 400));
            Laikini.Add(new Image<Hsv, byte>(400, 400));

            //Sukuriami paveiksleliai ir paimama centrinio pixelio spalva
            Laikini[1] = new Image<Hsv, byte>(400, 400);
            Laikini[1].SetValue(new Hsv(0, 0, 255));

            Laikini[0].Bitmap = (Bitmap)pav;
            Laikini[0] = Laikini[0].Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear);

            //Pakoreguojamas paveiksliukas, kad geriau veiktu atpazinimo algoritmas
            Laikini[0] = Laikini[0].SmoothBlur(7, 7);
            Laikini[0] = Laikini[0].Dilate(3);
            Laikini[0] = Laikini[0].Erode(5);

            //Gaunama centrinio pixelio spalva HSV koduoteje
            for (int i = 0; i < 3; i++)
            {
                Color[i] = Laikini[0].Data[200, 200, i];
            }

            SetColourBounds(Color);

            //Kitas budas atpazinimo, su kuriuo dar nezinau ka daryti, juodai baltas paveikslikas
            Hsv Color1 = new Hsv(Laikini[0].Data[200, 200, 0] - Alpha, Laikini[0].Data[200, 200, 1] - Alpha, Laikini[0].Data[200, 200, 2] - Alpha);
            Hsv Color2 = new Hsv(Laikini[0].Data[200, 200, 0] + Alpha, Laikini[0].Data[200, 200, 1] + Alpha, Laikini[0].Data[200, 200, 2] + Alpha);

            Image<Gray, byte> hsvimg = new Image<Gray, byte>(400, 400);
            hsvimg = Laikini[0].InRange(Color1, Color2);

            //Randami visi taškai ir piešiama kas matoma
            BeerTop(Laikini[0]);
            BeerBottom(Laikini[0]);
            BeerUpperLeftPoint(Laikini[0]);
            BeerLowerLeftPoint(Laikini[0]);
            BeerUpperRightPoint(Laikini[0]);
            BeerLowerRightPoint(Laikini[0]);

            //Nupiesiami paveiksliukai ir linijos ant jų
            Draw(Laikini[1]);
            Draw(Laikini[0]);

            //Paveiksliukai konvertuojami atgal i Image tipa is emgucv Image tipo
            Lines = Laikini[0].ToBitmap();
            Temp = Laikini[1].Bitmap;
            HSV = hsvimg.Bitmap;

            //Atlaisvinama atmintis
            Laikini[0].Dispose();
            Laikini[1].Dispose();
            hsvimg.Dispose();
            Laikini.RemoveRange(0, 2);
        }
    }
}
