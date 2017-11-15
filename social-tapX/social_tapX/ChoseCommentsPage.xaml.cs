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
		public ChoseCommentsPage ()
        {
            InitializeComponent();
            Backround.Source = MainPage.BackroundImage.Source;
            NewComment.Text = "Leave a Comment";
            ReadComment.Text = "Read Comments";
        }

        private void NewComment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Comments());
        }

        private void ReadComment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ReadCommentsPage());
        }
    }
}