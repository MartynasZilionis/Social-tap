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
    public partial class ReadCommentsPage_2 : ContentPage
    {

        private int Count;
        private int Stop = 0;
        private Rated_Bar Bar;
        public ReadCommentsPage_2(Rated_Bar bar)
        {
            Bar = bar;
            InitializeComponent();
            ListView.ItemsSource = App.WebSvc.GetListOfComments(Bar.Name ,999);





        }
        void OnCommentSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                
            }
        }

    }
}
