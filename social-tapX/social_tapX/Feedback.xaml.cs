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
	public partial class Feedback : ContentPage
	{
        public delegate void ThisIsDone(Object sender, EventArgs args);
        public static event ThisIsDone DoneEvent = delegate { };

        private string FeedBack;
        private string AuthToken;

		public Feedback (string authToken = null)
		{
            AuthToken = authToken;
			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;

            Title.Text = "I Challenge YOU! Roast Me ;)";
            Submit.Text = "Submit";
        }

        async private void Thankyou()
        {
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
            DoneEvent?.Invoke(this, new EventArgs());
        }

        private void Submit_Pressed(object sender, EventArgs e)
        {
            if(ComentOnUs.Text == null || ComentOnUs.Text == "")
            {
                Submit.Text = "Please :)";
                Submit.TextColor = Color.Red;
            }
            else
            {
                Title.IsVisible = false;
                ComentOnUs.IsVisible = false;
                ComentOnUs.IsEnabled = false;
                Submit.IsEnabled = false;
                Submit.IsVisible = false;
                FeedBack = ComentOnUs.Text;

                //App.WebSvc.Set_FeedbackAndDate(FeedBack, DateTime.Now);

                Thankyou();
            }
        }

        private void ComentOnUs_Focused(object sender, FocusEventArgs e)
        {
            Submit.Text = "Submit";
            Submit.TextColor = Color.OrangeRed;
        }
    }
}