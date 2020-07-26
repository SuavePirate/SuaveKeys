using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SuaveKeys.Clients.Views;

namespace SuaveKeys.Clients
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new LandingPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
