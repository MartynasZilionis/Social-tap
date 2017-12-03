using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
    delegate void BackEnabled(bool x);
    delegate void SetData(string name, string percent, string rating);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ratings : ContentPage
    {
        private Lazy<List<String>> Bname = new Lazy<List<String>>(() => new List<string>());
        private Lazy<List<String>> Bpercent = new Lazy<List<String>>(() => new List<string>());
        private Lazy<List<String>> Brating = new Lazy<List<String>>(() => new List<string>());
        private List<Tuple<String, String, String>> Bprop = new List<Tuple<String, String, String>>();
        
        private int Count;
        private int Stop = 0;
        private int load = 0;
        
        public Ratings (int backenabled = 0, int count = 20)
		{
            this.Count = count;
            
            InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;

            Name.Text = "Bar Name";
            Percent.Text = "%";
            Rating.Text = "Rating";

            Next.IsVisible = true;
            Next.IsEnabled = true;

            Next.Text = "More";
            LoadInData(load);
            Update();
        }

        void Update()
        {
            liste.ItemsSource = null;
            liste.ItemsSource = Bprop;
        }
        
        void LoadInData(int count)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i+count < App.WebSvc.GetListOfBars(Count).Count)
                {
                    Bprop.Add(Tuple.Create(App.WebSvc.GetListOfBars(Count).ElementAt(i+count).Name, App.WebSvc.GetListOfBars(Count).ElementAt(i+count).Percent, App.WebSvc.GetListOfBars(Count).ElementAt(i+count).Rating));
                }
                else
                {
                    Stop = 1;
                }
            }
            if (Stop == 1)
            {
                Next.IsEnabled = false;
                Next.IsVisible = false;
            }
        }
        
        private void Next_Pressed(object sender, EventArgs e)
        {
            load += 10;
            LoadInData(load);
            Update();
        }
    }
}