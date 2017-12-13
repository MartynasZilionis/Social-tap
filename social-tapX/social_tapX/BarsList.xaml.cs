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
    public partial class BarsList : ContentPage
    {
        private double Lat;
        private double Long;
        private string AuthToken;
        private Role Role;

        public BarsList(double Lat_, double Long_, Role role, string authToken = null)
        {
            Role = role;
            AuthToken = authToken;
            Lat = Lat_;
            Long = Long_;
            InitializeComponent();
            Backround.Source = MainPage.BackroundImage.Source;
            Start();
        }

        async private void Start()
        {

            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetAllBars(AuthToken);

            ListView.ItemsSource = Bars;
        }

        void OnBarSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                RestModels.Bar Bar = e.SelectedItem as RestModels.Bar;
                Navigation.PushAsync(new BarMainPage(Bar, Role, AuthToken));
            }
        }

    }
}
