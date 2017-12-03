﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBar : ContentPage
	{
        double Lat;
        double Long;
        String barName;
        RestModels.Bar Bar;
        RestModels.Coordinate coords;
		public AddBar (double Lat_, double Long_)
		{
            Lat = Lat_;
            Long = Long_;
			InitializeComponent ();
            SetInterface();
		}
        private void SetInterface()
        {
            Question.Text = "Type name of bar to add it";
            CreateBar.Text = "Add Bar";
            BarName.IsVisible = true;
            CreateBar.IsVisible = true;
            CreateBar.IsVisible = true;
            
        }
        async private void UploadBar()
        {
            Bar = new RestModels.Bar(barName, coords);
            await App.WebSvc.UploadBar(Bar);
        }
        private void AddBar_Pressed(object sender, EventArgs e)
        {
            if (BarName.Text != "" && BarName.Text != "Enter Bar name")
            {
                barName = BarName.Text;
                coords = new RestModels.Coordinate(Lat, Long);
                UploadBar();
                Navigation.PushAsync(new BarMainPage(Bar));
            }
            else
            {
                BarName.TextColor = Color.Red;
                BarName.Text = "Enter Bar name";
            }
        }
    }
}