using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ratings_2 : ContentPage
    {

        private double DeviceWidth;
        private double DeviceHeight;
        public Ratings_2()
        {
            InitializeComponent();
            Start();
        }

        async private void Start()
        {
            DeviceWidth = Application.Current.MainPage.Width;
            DeviceHeight = Application.Current.MainPage.Height;
            BarName.WidthRequest = DeviceWidth * 2 / 3;
            BarScore.WidthRequest = DeviceWidth / 3;
            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetAllBars();
            var SortedBars = Bars.OrderByDescending(c => c.Score);
            ListView.ItemsSource = SortedBars;
        }

        void OnBarSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                RestModels.Bar Bar = e.SelectedItem as RestModels.Bar;
                String AveragePrice = "Average Price in Bar: " + Bar.AveragePrice;
                String Score = "\n Bar Score: " + Bar.Score;
                String CommentCount = "\n Total Comments: " + Bar.CommentsCount;
                String RatingCount = "\n Total Ratings: " + Bar.RatingsCount;
                String BarInfo = AveragePrice + Score + CommentCount + RatingCount;
                DisplayAlert(Bar.Name, BarInfo, "OK");
            }
        }

    }
}
