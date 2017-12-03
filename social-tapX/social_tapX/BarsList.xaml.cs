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

        public BarsList(double Lat_, double Long_)
        {
            Lat = Lat_;
            Long = Long_;
            InitializeComponent();
            Start();
        }

        async private void Start()
        {
            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetAllBars();

            ListView.ItemsSource = Bars;
        }

        void OnBarSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                RestModels.Bar Bar = e.SelectedItem as RestModels.Bar;


                Navigation.PushAsync(new BarMainPage(Bar));
            }
        }

    }
}
