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
        private RestModels.Bar Bar;
        public ReadCommentsPage_2(RestModels.Bar bar)
        {
            Bar = bar;
            InitializeComponent();
            Start();
        }

        async private void Start()
        {
            IEnumerable<RestModels.Comment> Comments = await App.WebSvc.GetComments(Bar.Id, 0, 200);
            ListView.ItemsSource = Comments.ToString();
        }
        void OnCommentSelect(object sender, SelectedItemChangedEventArgs e)
        {

        }

    }
}
