﻿using System;
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
            Backround.Source = MainPage.BackroundImage.Source;
            Start();
        }

        async private void Start()
        {
            IEnumerable<RestModels.Bar> Bars = await App.WebSvc.GetNearestBars(new RestModels.Coordinate(Lat, Long), 50);

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
