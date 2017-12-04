using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace social_tapX
{
    public partial class BarMainPage : ContentPage
    {

        //public object LabelGeolocation { get; private set; }
        private social_tapX.RestModels.Bar Bar;
        private string BarName;

        public BarMainPage(RestModels.Bar bar)
        {
            Bar = bar;
            InitializeComponent();
            Backround.Source = MainPage.BackroundImage.Source;
            GetBarDetails();
            InitializeMainMenu();
            
        }

        private void GetBarDetails()
        {
            Title = BarName;
        }

        async private void DoneHandler(object sender, EventArgs y)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (ArgumentOutOfRangeException e)
            {
                //its all good here, just skip it
            }
        }

        async void InitializeMainMenu()
        {
            Question.Text = "Would You Like To:";
            Question.IsVisible = true;
            GetLocation.Text = "Get your location";
            Picture.Text = "Take a Picture";
            Comment.Text = "Comment";
            Rating.Text = "View Top Rated";
            Button[] Buttons = { Picture, Comment, Rating};
            foreach (Button B in Buttons)
            {
                B.IsVisible = true;
            }

            for (double i = 0; i < 1; i += 0.017)
            {
                Question.Opacity = i;
                foreach (Button B in Buttons)
                {
                    B.Opacity = i;
                }
                await Task.Delay(25);
            }
        }

        private async void Location_Pressed(object sender, EventArgs e)
        {
            TimeSpan timeout = new TimeSpan(0, 0, 10);
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync(timeout);
                    LocationShow.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                LocationShow.Text = "Error: " + ex;
            }

        }

        private void Picture_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPicture(Bar));
        }

        private void Comment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChoseCommentsPage(Bar));
        }

        private void Rating_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Ratings_2(Bar));
        }
    }
}
