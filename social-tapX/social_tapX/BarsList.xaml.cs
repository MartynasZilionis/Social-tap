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

        private int Count;
        private int Stop = 0;
        public BarsList()
        {
            InitializeComponent();
            ListView.ItemsSource = App.WebSvc.GetListOfBars(999);





        }
        void OnBarSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var Bar = e.SelectedItem as Rated_Bar;
                Navigation.PushAsync(new BarMainPage(Bar.Name));
            }
        }

    }
}
