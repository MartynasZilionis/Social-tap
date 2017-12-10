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
	public partial class ChoseCommentsPage : ContentPage
	{
        private RestModels.Bar Bar;
        private string AuthToken;
		public ChoseCommentsPage (RestModels.Bar bar, string authToken = null)
        {
            AuthToken = authToken;
            Bar = bar;
            InitializeComponent();
            Backround.Source = MainPage.BackroundImage.Source;
            NewComment.Text = "Leave a Comment";
            ReadComment.Text = "Read Comments";
        }

        private void NewComment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Comments(Bar, AuthToken));
        }

        private void ReadComment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReadCommentsPage_2(Bar, AuthToken));
        }
    }
}