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
            Backround.Source = MainPage.BackroundImage.Source;
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
            DeviceWidth = Application.Current.MainPage.Width;
            DeviceHeight = Application.Current.MainPage.Height;
            BarName.WidthRequest = DeviceWidth * 2 / 3;
            BarScore.WidthRequest = DeviceWidth / 3;
            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetTopBars(0, 20);
            if (!IsNullOrEmpty<RestModels.Bar>(Bars))
            {
                var SortedBars = Bars.OrderByDescending(c => c.AverageFill);
                ListView.IsVisible = true;
                ListView.IsEnabled = true;
                ListView.ItemsSource = SortedBars;
            }
            else
            {
                EmptyList.Text = "There are currently bars with raiting";
                EmptyList.IsEnabled = true;
                EmptyList.IsVisible = true;
            }
        }
        async private void StartOneBar()
        {
            BarName.Text = "Rated date";
            BarScore.Text = "Percentage Filled";
            DeviceWidth = Application.Current.MainPage.Width;
            DeviceHeight = Application.Current.MainPage.Height;
            BarName.WidthRequest = DeviceWidth * 2 / 3;
            BarScore.WidthRequest = DeviceWidth / 3;
            try
            {
                IEnumerable<RestModels.Rating> Ratings = await App.WebSvc.GetRatings(Bar.Id, 0, 200);

                if (!IsNullOrEmpty<RestModels.Rating>(Ratings))
                {
                    ListViewBar.IsVisible = true;
                    ListViewBar.IsEnabled = true;
                    var SortedBars = Ratings.OrderByDescending(c => c.Date);
                    ListViewBar.ItemsSource = SortedBars;
                }
                else
                {
                    EmptyList.Text = "There are currently no ratings for this bar";
                    EmptyList.IsEnabled = true;
                    EmptyList.IsVisible = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caughtt.", e);
            }
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
                String Score = "\n Bar Score: " + Bar.AverageFill;
                String CommentCount = "\n Total Comments: " + Bar.CommentsCount;
                String RatingCount = "\n Total Ratings: " + Bar.RatingsCount;
                String BarInfo = AveragePrice + Score + CommentCount + RatingCount;
                DisplayAlert(Bar.Name, BarInfo, "OK");
            }
        }
        private static bool IsNullOrEmpty<T>(IEnumerable<T> source)
        {
            if (source != null)
            {
                foreach (T obj in source)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
