﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace social_tapX
{
	public partial class App : Application
	{
        public static IWebService WebSvc { get; private set; }
        public App ()
		{
			InitializeComponent();
            WebSvc = new WebService();
			MainPage = new NavigationPage (new social_tapX.LoginPage());
		}


		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
