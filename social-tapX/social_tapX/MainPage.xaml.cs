using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace social_tapX
{
	public partial class MainPage : ContentPage
	{
        public string Name;
        static public Image BackroundImage;

        public MainPage()
        {
            InitializeComponent();
            GetImage();
            Initialize();
        }

        async void Initialize()
        {
            InitializeBackround();
            await Task.Delay(5000);
            InitializeMainMenu();
        }

        async void InitializeBackround()
        {
            Backround.Source = BackroundImage.Source;
            Backround.Opacity = 0;
            Backround.IsVisible = true;

            for (double i = 0; i < 1; i += 0.017)
            {
                Backround.Opacity = i;
                 await Task.Delay(50);
            }

            for (double i = 1; i > 0.25; i -= 0.017)
            {
                Backround.Opacity = i;
                await Task.Delay(50);
            }
        }

        void GetImage()
        {
            BackroundImage = new Image { Source = "config\\logo.jpg" };
        }


        async void InitializeMainMenu()
        {
            
            Question.Text = "Would You Like To:";
            Question.IsVisible = true;
            Picture.Text = "Take a Picture";
            Comment.Text = "Comment";
            Rating.Text = "View Top Rated";
            Feedback.Text = "Feedback :)";
            Button[] Buttons = { Picture, Comment, Rating, Feedback };
            foreach (Button B in Buttons)
                {
                    B.IsVisible = true;
                }

            for (double i = 0; i < 1; i += 0.017)
            {
                Question.Opacity = i;
                foreach (Button B in Buttons)
                {
                    B.Opacity = i;
                }
                await Task.Delay(25);
            }
        }

        private void Picture_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPicture());
        }

        private void Comment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChoseCommentsPage());
        }

        private void Rating_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Ratings());
        }

        private void Feedback_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Feedback());
        }

        private void DEBUG_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DEBUG());
        }
    }
}
