using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPicture : ContentPage
	{
        private string Name = "";
        private int SamePhoto = 0;

		public NewPicture ()
		{
			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;
		}

        private void FromFile_Pressed(object sender, EventArgs e)
        {
            Photo.Source = "config\\backround.jpg";
            OK.IsEnabled = true;
            OK.IsVisible = true;
            BarName.Text = Name;
            SamePhoto = 0;
        }

        private void FromCamera_Pressed(object sender, EventArgs e)
        {
            Photo.Source = "config\\logo.jpg";
            OK.IsEnabled = true;
            OK.IsVisible = true;
            BarName.Text = Name;
            SamePhoto = 0;
        }

        private void OK_Pressed(object sender, EventArgs e)
        {
            if (SamePhoto == 0)
            {
                if (BarName.Text == "")
                {
                    BarName.Placeholder = "Please Enter Bar Name Here";
                    BarName.PlaceholderColor = Color.Red;
                }
                else
                {
                    Name = BarName.Text;
                    int percent = Recognition.Recognize();
                    App.WebSvc.Set_BarAndPercent(BarName.Text, percent);
                    BarName.Text = "There Is " + percent + "% Beer In The Mug!";
                    SamePhoto = 1;
                }
            }
            else
            {
                BarName.Text = "";
                BarName.Placeholder = "Please Chose Another Photo";
                BarName.PlaceholderColor = Color.Red;
            }
        }
    }
}