using Android.Graphics;
using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace social_tapX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPicture : ContentPage
	{
        private string Name = "";
        private int SamePhoto = 0;
        private Bitmap BM;
        private string BarWithName;

		public NewPicture (string barWithName)
		{
            BarWithName = barWithName;
			InitializeComponent ();
            Backround.Source = MainPage.BackroundImage.Source;
            GetBarName(BarWithName);
		}

        void GetBarName(string BarName)
        {
            ShowBarName.Text = "Adding photo for bar " + BarName;
            ShowBarName.IsVisible = true;
        }

        void ExportBitmapAsPNG(Bitmap bitmap)
        {
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pvz.jpg");
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
        }

        private async void FromCamera_Pressed(object sender, EventArgs e)
        {
            ImageSource _photo = "ratingstar.png";

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null)
            {
                try
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pvz.jpg"));
                }
                catch (System.IO.FileNotFoundException y)
                {

                }

                _photo = ImageSource.FromStream(() => { return photo.GetStream(); });
                Photo.Source = _photo;

                BM = await social_tapX.ImageBitmapHandler.GetImageFromImageSource(_photo, Xamarin.Forms.Forms.Context);

                OK.IsEnabled = true;
                OK.IsVisible = true;
                BarName.Text = Name;
                SamePhoto = 0;

            }
            else
            {
                BarName.Text = "ERROR";
            }
        }

        private void OK_Pressed(object sender, EventArgs e)
        {
            if (SamePhoto == 0)
            {
                Name = BarName.Text;
                social_tapX.Recognition RC = social_tapX.Recognition.GetRecognition();
                RC.Recognize(BM, 60);
                int percent = RC.Proc;
                 
                BM = RC.BITMAP;

                ExportBitmapAsPNG(BM);

                Photo.Source = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pvz.jpg");

                App.WebSvc.Set_BarAndPercent(BarName.Text, percent);
                BarName.IsVisible = true;
                BarName.Text = "There Is " + percent + "% Beer In The Mug!";
                SamePhoto = 1;
            }
            else
            {
                BarName.IsVisible = true;
                BarName.Text = "Please Chose Another Photo";
                BarName.TextColor = Xamarin.Forms.Color.Red;
            }
        }
    }
}