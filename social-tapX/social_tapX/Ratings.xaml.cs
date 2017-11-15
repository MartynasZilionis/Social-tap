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
    public partial class Ratings : ContentPage
    {
        private List<String> Bname = new List<string>();
        private List<String> Bpercent = new List<string>();
        private List<String> Brating = new List<string>();
        private int Count;
        private int BackEnabled;
        private int Stop = 0;

        public Ratings (int backenabled = 0, int count = 20)
		{
            this.Count = count;
            this.BackEnabled = backenabled;

			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;

            Name.Text = "Bar Name";
            Percent.Text = "%";
            Rating.Text = "Rating";

            if (BackEnabled == 1)
            {
                Back.IsEnabled = true;
                Back.IsVisible = true;
            }
            else
            {
                Back.IsEnabled = false;
                Back.IsVisible = false;
            }
            Next.IsVisible = true;
            Next.IsEnabled = true;

            Next.Text = "Next";
            Back.Text = "Back";

            LoadInData();
            Update();
        }

        void Update()
        {
            BarName.ItemsSource = Bname.ToArray();
            BarPercent.ItemsSource = Bpercent.ToArray();
            BarRating.ItemsSource = Brating.ToArray();
        }

        void LoadInData()
        {
            for (int i = 0; i < 20; i++)
            {
                if (i < App.WebSvc.GetListOfBars(Count).Count)
                {
                    Bname.Add(App.WebSvc.GetListOfBars(Count).ToArray()[i].Name);
                    Bpercent.Add(App.WebSvc.GetListOfBars(Count).ToArray()[i].Percent);
                    Brating.Add(App.WebSvc.GetListOfBars(Count).ToArray()[i].Rating);
                }
                else
                {
                    Bname.Add("");
                    Bpercent.Add("");
                    Brating.Add("");
                    Stop = 1;
                }
            }
            if (Stop == 1)
            {
                Next.IsEnabled = false;
                Next.IsVisible = false;
            }
        }

        private void Next_Pressed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Ratings(1, Count + 20));
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}