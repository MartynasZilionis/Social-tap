using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
    delegate void BackEnebled(bool x);

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BarComments : ContentPage
	{
        private string Bar_Name;
        private List<Label> Labels = null;
        private int Count;
        private int BackIsEnabled;        

        private void backenabled(bool x)
        {
            Back.IsEnabled = x;
            Back.IsVisible = x;
        }

        public BarComments(string Bar_Name, int count = 10, int backisenabled = 0)
		{
            Count = count;
            BackIsEnabled = backisenabled;
			InitializeComponent ();

            BackEnebled BE = backenabled;

            Backround.Source = MainPage.BackroundImage.Source;
            BarName.Text = Bar_Name;
            this.Bar_Name = Bar_Name;
            Labels = new List<Label> { C_1, C_2, C_3, C_4, C_5, C_6, C_7, C_8, C_9, C_10};

            if (BackIsEnabled == 1)
            {
                BE(true);
            }
            else
            {
                BE(false);
            }
            Next.IsVisible = true;
            Next.IsEnabled = true;

            Next.Text = "Next";
            Back.Text = "Back";

            LoadComments(Bar_Name);
        }

        private void LoadComments(string Bar_Name)
        {
            int i = 0;
            foreach (Label L in Labels){
                L.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));

                if (i < App.WebSvc.GetListOfComments(Bar_Name, Count).Count)
                {
                    L.Text = App.WebSvc.GetListOfComments(Bar_Name, Count).ElementAt(i);
                }
                else
                {
                    L.Text = "";
                }
                i++;
            }
        }

        private void Next_Pressed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BarComments(Bar_Name, Count + 10, 1));
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private delegate void BackDelegate(bool x);
    }
}