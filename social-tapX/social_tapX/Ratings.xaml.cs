using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
    delegate void BackEnabled(bool x);
    delegate void SetData(string name, string percent, string rating);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ratings : ContentPage
    {
        private Lazy<List<String>> Bname = new Lazy<List<String>> (() => new List<string>());
        private Lazy<List<String>> Bpercent = new Lazy<List<String>>(() => new List<string>());
        private Lazy<List<String>> Brating = new Lazy<List<String>>(() => new List<string>());
        private int Count;
        private int BackEnabled;
        private int Stop = 0;

        private void backisenabled(bool x)
        {
            Back.IsEnabled = x;
            Back.IsVisible = x;
        }

        public Ratings (int backenabled = 0, int count = 20)
		{
            this.Count = count;
            this.BackEnabled = backenabled;

            BackEnabled BE = backisenabled;

            InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;

            Name.Text = "Bar Name";
            Percent.Text = "%";
            Rating.Text = "Rating";

            if (BackEnabled == 1)
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

            LoadInData();
            Update();
        }

        void Update()
        {
            BarName.ItemsSource = Bname.Value.ToArray();
            BarPercent.ItemsSource = Bpercent.Value.ToArray();
            BarRating.ItemsSource = Brating.Value.ToArray();
        }

        void LoadInData()
        {
            SetData SD = delegate (string name, string percent, string rating)
            {
                Bname.Value.Add(name);
                Bpercent.Value.Add(percent);
                Brating.Value.Add(rating);
            };

            for (int i = 0; i < 20; i++)
            {
                if (i < App.WebSvc.GetListOfBars(Count).Count)
                {
                    SD(App.WebSvc.GetListOfBars(Count).ElementAt(i).Name, App.WebSvc.GetListOfBars(Count).ElementAt(i).Percent, App.WebSvc.GetListOfBars(Count).ElementAt(i).Rating);
                }
                else
                {
                    SD("", "", "");
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