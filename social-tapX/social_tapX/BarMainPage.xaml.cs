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
        private RestModels.Bar Bar;
        private string BarName;
        private string AuthToken;
        private Role Role;

        public BarMainPage(RestModels.Bar bar, Role role, string authToken = null)
        {
            AuthToken = authToken;
            Role = role;
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
            catch (ArgumentOutOfRangeException)
            {
                //its all good here, just skip it
            }
        }

        void InitializeMainMenu()
        {
            Question.Text = "Would You Like To:";
            Question.IsVisible = true;
            GetLocation.Text = "Get your location";
            Picture.Text = "Take a Picture";
            Comment.Text = "Comment";
            Rating.Text = "View Top Ratings";
            Button[] Buttons = { Picture, Comment, Rating};
            foreach (Button B in Buttons)
            {
                if (Role == Role.Anonymous && (B == Picture))
                {
                    B.IsEnabled = false;
                    B.IsVisible = false;
                }
                else
                {
                    B.IsVisible = true;
                    B.Opacity = 1;
                }

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
            Navigation.PushAsync(new NewPicture(Bar, AuthToken));
        }

        private void Comment_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChoseCommentsPage(Bar, AuthToken));
        }

        private void Rating_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Ratings_2(Bar, AuthToken));
        }
    }
}
