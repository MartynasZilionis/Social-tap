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

        public Ratings (int backenabled = 0, int count = 20)
		{
            this.Count = count;
            this.BackEnabled = backenabled;

			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;

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
            BarsToList.MakeList();
            for (int i = 0; i < 20; i++)
            {
                Bname.Add(BarsToList.GetListOfBars().ToArray()[i].Name);
                Bpercent.Add(BarsToList.GetListOfBars().ToArray()[i].Percent);
                Brating.Add(BarsToList.GetListOfBars().ToArray()[i].Rating);
            }
        }

        async private void Next_Page(int x, int y)
        {
            await Navigation.PushAsync(new Ratings(x, y));
        }

        async private void Back_Page()
        {
            await Navigation.PopAsync();
        }

        private void Next_Pressed(object sender, EventArgs e)
        {
            Next_Page(1, Count * 2);
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            Back_Page();
        }
    }
}