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
        private RestModels.Bar Bar;
        public Ratings_2()
        {
            InitializeComponent();
            StartAllBars();

        }
        public Ratings_2(RestModels.Bar bar)
        {
            Bar = bar;
            InitializeComponent();
            StartOneBar();
        }

        async private void StartAllBars()
        {
            BarName.Text = "Bar Name";
            BarScore.Text = "Bar Score";
            ListView.IsVisible = true;
            ListView.IsEnabled = true;
            DeviceWidth = Application.Current.MainPage.Width;
            DeviceHeight = Application.Current.MainPage.Height;
            BarName.WidthRequest = DeviceWidth * 2 / 3;
            BarScore.WidthRequest = DeviceWidth / 3;
            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetAllBars();
            var SortedBars = Bars.OrderByDescending(c => c.Score);
            ListView.ItemsSource = SortedBars;
        }
        async private void StartOneBar()
        {
            BarName.Text = "Rated date";
            BarScore.Text = "Percentage Filled";
            ListViewBar.IsVisible = true;
            ListViewBar.IsEnabled = true;
            DeviceWidth = Application.Current.MainPage.Width;
            DeviceHeight = Application.Current.MainPage.Height;
            BarName.WidthRequest = DeviceWidth * 2 / 3;
            BarScore.WidthRequest = DeviceWidth / 3;
            IEnumerable<RestModels.Rating> Ratings = await App.WebSvc.GetRatings(Bar.Id, 0, 200);
            var SortedBars = Ratings.OrderByDescending(c => c.Date);
            ListViewBar.ItemsSource = SortedBars;
        }
        private void OnRatingSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                RestModels.Rating Rating = e.SelectedItem as RestModels.Rating;
                String MugSize = "\n Mug Size: " + Rating.MugSize;
                String MugPrice = "\n Mug Price: " + Rating.MugPrice;
                String FillPercent = "\n Fill Percentage: " + Rating.FillPercentage;
                String Date = "\n Date: " + Rating.Date;
                String BarInfo = Date + MugSize + MugPrice + FillPercent;
                DisplayAlert(Bar.Name, BarInfo, "OK");
            }
        }

        private void OnBarSelect(object sender, SelectedItemChangedEventArgs e)
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
