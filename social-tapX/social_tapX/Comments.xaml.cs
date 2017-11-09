﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Comments : ContentPage
	{
        private string barname;
        private string comment;
        private int Rating = 0;

        private Image[] Star;
        private int RatingFlag = 0;

        public Comments ()
		{
			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;
            Star = new List<Image> { Star1_I, Star2_I, Star3_I, Star4_I, Star5_I, Star6_I, Star7_I, Star8_I, Star9_I, Star10_I }.ToArray();
            
            foreach (Image S in Star)
            {
                S.Source = "config\\ratingstar.png";
            }
        }

        async private void OK_Pressed(object sender, EventArgs e)
        {
            if ((BarName.Text != "") && (Comment.Text != "") && (Comment.Text != "Please Enter Your Comment Here"))
            {
                this.barname = BarName.Text;
                this.comment = Comment.Text;

                BarName.IsVisible = false;
                BarName.IsEnabled = false;
                Comment.IsVisible = false;
                Comment.IsEnabled = false;
                OK.IsEnabled = false;
                OK.IsVisible = false;
                
                foreach (Image S in Star)
                {
                    S.IsVisible = false;
                }

                foreach (Button B in new List<Button> { Star1_B, Star2_B, Star3_B, Star4_B, Star5_B, Star6_B, Star7_B, Star8_B, Star9_B, Star10_B }.ToArray())
                {
                    B.IsEnabled = false;
                    B.IsVisible = false;
                }

                ThankYou.IsVisible = true;
                ThankYou.Text = "Thank You :)";
                for (double i = 0; i < 1; i += 0.017)
                {
                    ThankYou.Opacity = i;
                    await Task.Delay(30);
                }
                await Task.Delay(500);
                for (double i = 1; i > 0; i -= 0.017)
                {
                    ThankYou.Opacity = i;
                    await Task.Delay(20);
                }
                ThankYou.IsVisible = false;
                await Navigation.PopAsync();
            }
            else if (BarName.Text == "")
            {
                BarName.PlaceholderColor = Color.Red;
                BarName.Placeholder = "Please Enter Bar Name";                
            }
            else if (Rating != 0)
            {
                Comment.TextColor = Color.Red;
                Comment.Text = "Please Enter Your Comment Here";
            }
            else 
            {
                OK.Text = "Please Leave a Rating :)";
                OK.TextColor = Color.Red;
                RatingFlag = 1;
            }
        }

        void ChangeRating(int x)
        {
            Rating = x;
            for(int i = 0; i < x; i++)
            {
                Star[i].Source = "config\\beerrating.png";
            }

            for (int i = x; i < 10; i++)
            {
                Star[i].Source = "config\\ratingstar.png";
            }
        }

        private void Rated()
        {
            OK.Text = "OK";
            OK.TextColor = Color.LimeGreen;
            RatingFlag = 0;
        }

        private void Comment_Focused(object sender, FocusEventArgs e)
        {
            Comment.TextColor = Color.Black;
        }

        private void Star1_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(1);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star2_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(2);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star3_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(3);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star4_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(4);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star5_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(5);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star6_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(6);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star7_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(7);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star8_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(8);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star9_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(9);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }

        private void Star10_B_Pressed(object sender, EventArgs e)
        {
            ChangeRating(10);
            if (RatingFlag == 1)
            {
                Rated();
            }
        }
    }
}