using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReadCommentsPage : ContentPage
	{
        private List<String> Bname = new List<string>();
        private int Count;
        private int BackEnabled;
        private string Bar_Name;
        private int Stop = 0;

        public ReadCommentsPage (int backenabled = 0, int count = 20)
        {
            this.Count = count;
            this.BackEnabled = backenabled;

            InitializeComponent();
            Backround.Source = MainPage.BackroundImage.Source;

            Name.Text = "Bar Name";

            if (BackEnabled == 1)
            {
                Back.IsEnabled = true;
                Back.IsVisible = true;
            }
            else
            {
                Back.IsEnabled = false;
                Back.IsVisible = false;
            }
            Next.IsVisible = true;
            Next.IsEnabled = true;

            Next.Text = "Next";
            Back.Text = "Back";

            LoadInData();
            Update();
        }

        void Update()
        {
            BarName.ItemsSource = Bname.ToArray();
        }

        void LoadInData()
        {
            for (int i = 0; i < 20; i++)
            {
                if (i < WebService.GetListOfBars(Count).Count) Bname.Add(WebService.GetListOfBars(Count).ToArray()[i].Name);
                else
                {
                    Bname.Add("");
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
            Bar_Name = "";
            OK.IsVisible = false;
            OK.IsEnabled = false;
            ChosenBar.Text = "";
            ChosenBar.IsVisible = false;
            Navigation.PushModalAsync(new ReadCommentsPage(1, Count + 20));
        }

        private void Back_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void BarName_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Bar_Name = BarName.SelectedItem.ToString();
            ChosenBar.Text = Bar_Name;
            ChosenBar.IsVisible = true;
            OK.Text = "OK";
            OK.IsEnabled = true;
            OK.IsVisible = true;
        }

        private void OK_Pressed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BarComments(Bar_Name));
        }
    }
}