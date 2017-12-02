using System;
using System.Linq;
using Emgu.CV.Structure;
using Android.Graphics;
using Emgu.CV;

namespace social_tapX
{
    public class Recognition
    {
        //SINGLETON
        private static Recognition Pointer = null;

        public static Recognition GetRecognition()
        {
            if (Pointer == null)
            {
                Pointer = new Recognition();
                return Pointer;
            }
            else return Pointer;
        }

        private Recognition() { }
        //SINGLETON END

        public Bitmap BITMAP { get; private set; }
        public int Proc { get; private set; }
        //Edit paveiksliukai, palengvinantys testavima, 0 - ka randa programa, 1 - kaip perdaromas paveiksliukas

        private byte[] Color = Enumerable.Repeat<byte>(0, 3).ToArray();         //Centrinio pixelio spalva  

        //Koreguota pikelio spalva, naudojama ifui, siandien per paskaita suzinojau apie HVS, manau reikes perdaryt
        private int[] CPlus = Enumerable.Repeat(0, 3).ToArray();
        private int[] CMinus = Enumerable.Repeat(0, 3).ToArray();

        //Kokia paklaida nuo centrinio pixelio spalvos užskaityt pixelius kaip tinkamus
        private int Alpha;

        private int xv = 0, yv = 200; // virsutinis taskas
        private int xa = 0, ya = 200; // apatinis taskas
        private int xvd = 0, yvd = 200; // virsus desine
        private int xad = 0, yad = 200; // apacia desine
        private int xvk = 0, yvk = 200; // virsus kaire
        private int xak = 0, yak = 200; // apacia kaire
        private int vk = 0, vd = 0; // virsutiniai taskai

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
            X.Draw(new LineSegment2D(new System.Drawing.Point(yvk, xvk), new System.Drawing.Point(yvd, xvd)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(yvk, xvk), new System.Drawing.Point(yak, xak)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(yak, xak), new System.Drawing.Point(ya, xa)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(ya, xa), new System.Drawing.Point(yad, xad)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(yad, xad), new System.Drawing.Point(yvd, xvd)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(vd, 0), new System.Drawing.Point(yvd, xvd)), new Hsv(155, 255, 255), 1);
            X.Draw(new LineSegment2D(new System.Drawing.Point(vk, 0), new System.Drawing.Point(yvk, xvk)), new Hsv(155, 255, 255), 1);
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

                }
                else
                {
                    xv = x;
                    yv = 200;
                    break;
                }
            }
        }

        //Alaus apacia
        private void BeerBottom(Image<Hsv, Byte> X)
        {
            for (int x = 200; x < 400; x++)
            {
                if ((X.Data[x, 200, 0] <= CPlus[0] && X.Data[x, 200, 0] >= CMinus[0]) &&
                    (X.Data[x, 200, 1] <= CPlus[1] && X.Data[x, 200, 1] >= CMinus[1]) &&
                    (X.Data[x, 200, 2] <= CPlus[2] && X.Data[x, 200, 2] >= CMinus[2]))
                {

                }
                else
                {
                    xa = x;
                    yv = 200;
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

        private void Reset()
        {
            BITMAP = null;
            xv = 0; yv = 0;
            xa = 0; ya = 200;
            xvd = 0; yvd = 200;
            xad = 0; yad = 200;
            xvk = 0; yvk = 200;
            xak = 0; yak = 200;
            vk = 0; vd = 0;
        }

        public void Recognize(Bitmap X, byte skirtumas)
        {
            Reset();

            //Nustatoma paklaida
            Alpha = skirtumas;
            BITMAP = X;

            Image<Hsv, byte> Edit = new Image<Hsv, byte>(400, 400)
            {
                Bitmap = X
            };
            Edit = Edit.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear);

            //Pakoreguojamas paveiksliukas, kad geriau veiktu atpazinimo algoritmas
            Edit = Edit.SmoothBlur(7, 7);
            Edit = Edit.Dilate(3);
            Edit = Edit.Erode(5);

            //Gaunama centrinio pixelio spalva HSV koduoteje
            for (int i = 0; i < 3; i++)
            {
                Color[i] = Edit.Data[200, 200, i];
            }

            SetColourBounds(Color);

            //Randami visi taškai ir piešiama kas matoma
            BeerTop(Edit);
            BeerBottom(Edit);
            BeerUpperLeftPoint(Edit);
            BeerLowerLeftPoint(Edit);
            BeerUpperRightPoint(Edit);
            BeerLowerRightPoint(Edit);

            //Virsutiniu tasku radimas
            double m1 = ((double)yvk - yak) / (xvk - (double)xak);
            double b1 = yvk - m1 * (double)xvk;
            vk = (int)b1;

            double m2 = ((double)yvd - yad) / (xvd - (double)xad);
            double b2 = yvd - m2 * (double)xvd;
            vd = (int)b2;

            //Nupiesiami paveiksliukai ir linijos ant jų
            Draw(Edit);

            Image<Hsv, byte> ST = new Image<Hsv, byte>(400, 400, new Hsv(0, 0, 0));
            Draw(ST);

            //Ploto apskaiciavimas
            int beer = 0;
            int glass = 0;

            int a = 0;

            //Tuscios stiklines
            for (int x = 0; x < xv; x++)
            {
                for (int y = 0; y < 400; y++)
                {
                    if ((ST.Data[x, y, 0] == 155) &&
                        (ST.Data[x, y, 1] == 255) &&
                        (ST.Data[x, y, 2] == 255))
                    {
                        if (a == 0) a++;
                        else a = 0;
                    }
                    else if (a == 1)
                    {
                        glass++;
                    }
                }
            }

            //alaus
            a = 0;
            for (int x = xv + 1; x < xa - 1; x++)
            {
                for (int y = 0; y < 400; y++)
                {
                    if ((ST.Data[x, y, 0] == 155) &&
                        (ST.Data[x, y, 1] == 255) &&
                        (ST.Data[x, y, 2] == 255))
                    {
                        if (a == 0) a++;
                        else a = 0;

                    }
                    else if (a == 1)
                    {
                        beer++;
                    }
                }
            }

            //HSV = ST.Bitmap;
            Proc = (int)(((double)beer * (double)100) / (double)((double)beer + (double)glass));
            BITMAP = Edit.Bitmap;

            X.Dispose();
            ST.Dispose();
        }
    }
}
