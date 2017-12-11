using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace social_tapX
{
    public partial class MainPage : ContentPage
	{
        public string Name;
        public static Image BackroundImage;
        public static int WidthIs = 0;
        public static int HeightIs = 0;
        private int timeout;
        private double Lat;
        private double Long;
        private string AuthToken;
        private int Role;

        //public object LabelGeolocation { get; private set; }

        public MainPage(int role, string authToken = null)
        {
            AuthToken = authToken;
            Role = role;
            InitializeComponent();
            if (Role != 2)
            {
                NewBar.IsEnabled = true;
            }
            Feedback.DoneEvent += DoneHandler;
            Page ForSize = new ContentPage();
            WidthIs = (int)ForSize.Width;
            HeightIs = (int)ForSize.Height;
            ForSize = null;
            GetImage();
            Initialize();
        }
        


        async private void DoneHandler(object sender, EventArgs y)
        {
            try {
                await Navigation.PopAsync();
            }
            catch (ArgumentOutOfRangeException e)
            {
                //its all good here, just skip it
            }
        }

        async void Initialize()
        {
            InitializeBackround();
            await Task.Delay(5000);
            InitializeMainMenu();
        }

        async void InitializeBackround()
        {
            Backround.Source = BackroundImage.Source;
            
            Backround.Opacity = 0;
            Backround.IsVisible = true;
            Backround.HeightRequest = HeightIs;
            Backround.WidthRequest = WidthIs;

            for (double i = 0; i < 1; i += 0.017)
            {
                Backround.Opacity = i;
                 await Task.Delay(50);
            }

            for (double i = 1; i > 0.25; i -= 0.017)
            {
                Backround.Opacity = i;
                await Task.Delay(50);
            }
        }

        void GetImage()
        {
            BackroundImage = new Image { Source = "logo.jpg" };
            BackroundImage.WidthRequest = WidthIs;
            BackroundImage.HeightRequest = HeightIs;
        }


        async void InitializeMainMenu()
        {
            Question.Text = "Would You Like To:";
            Question.IsVisible = true;
            ExistingBar.Text = "Choose Bar";
            NewBar.Text = "Add Bar";
            Feed_back.Text = "Feedback :)";
            Rating.Text = "Top Rated Bars";
            Button[] Buttons = { ExistingBar, NewBar, Feed_back, Rating};
            foreach (Button B in Buttons)
            {
                if (Role == 0 && (B == NewBar || B == Feed_back))
                {
                    B.IsVisible = false;
                    B.IsEnabled = false;
                }
                else if (Role == 1 && B == NewBar)
                {
                    B.IsEnabled = false;
                    B.IsVisible = false;
                }
                else
                {
                    B.IsVisible = true;
                }
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

        private async Task GatherLocation()
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
                    Lat = results.Latitude;
                    Long = results.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                //Exeption log here
            }

        }

        private async void ExistingBar_Pressed(object sender, EventArgs e)
        {
            await GatherLocation();
            await Navigation.PushAsync(new BarsList(Lat, Long, Role, AuthToken));
        }

        private async void NewBar_Pressed(object sender, EventArgs e)
        {
            await GatherLocation();
            await Navigation.PushAsync(new AddBar(Lat, Long, Role, AuthToken));
        }

        private void Feedback_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Feedback(AuthToken));
        }
        private void Rating_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Ratings_2(AuthToken));
        }

    }
}
