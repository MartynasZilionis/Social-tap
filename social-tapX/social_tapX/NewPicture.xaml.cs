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
        }

        private void FromCamera_Pressed(object sender, EventArgs e)
        {
            Photo.Source = "config\\logo.jpg";
            OK.IsEnabled = true;
            OK.IsVisible = true;
        }

        private void OK_Pressed(object sender, EventArgs e)
        {
            //To DO
        }
    }
}