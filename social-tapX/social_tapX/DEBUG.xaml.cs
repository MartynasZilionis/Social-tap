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
	public partial class DEBUG : ContentPage
	{
		public DEBUG ()
		{
			InitializeComponent ();
            X1.Text = WebService.BarP.BarName;
            X2.Text = "" + WebService.BarP.Percent;
            X3.Text = WebService.BarCR.BarName;
            X4.Text = WebService.BarCR.Comment;
            X5.Text = "" + WebService.BarCR.Rating;
            X6.Text = WebService.FBD.Feedback;
            X7.Text = WebService.FBD.Date.ToString();
		}

	}
}