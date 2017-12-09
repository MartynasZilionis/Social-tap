using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
	public partial class LoginPage : ContentPage
	{

        public static Image BackroundImage;
        public static int WidthIs = 0;
        public static int HeightIs = 0;
        private string ClientId = "246497305885398";

        public LoginPage ()
		{
			InitializeComponent ();
            Page ForSize = new ContentPage();
            WidthIs = (int)ForSize.Width;
            HeightIs = (int)ForSize.Height;
            ForSize = null;
            GetImage();
            Initialize();
        }
        void GetImage()
        {
            BackroundImage = new Image { Source = "logo.jpg" };
            BackroundImage.WidthRequest = WidthIs;
            BackroundImage.HeightRequest = HeightIs;
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
            Backround.HeightRequest = HeightIs;
            Backround.WidthRequest = WidthIs;

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
        async void InitializeMainMenu()
        {
            Question.Text = "Would You Like To:";
            Question.IsVisible = true;
            Login.Text = "Login with Facebook";
            DoNotLogIn.Text = "Continue without Login";
            Button[] Buttons = { Login, DoNotLogIn};
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
        private void LoginPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FacebookProfilePage());
        }
        private void DoNotLogInPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage(null, 0));
        }

    }
}