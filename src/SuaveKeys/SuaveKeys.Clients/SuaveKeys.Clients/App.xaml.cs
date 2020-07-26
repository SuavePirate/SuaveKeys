using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SuaveKeys.Clients.Views;
using SuaveKeys.Clients.Services;
namespace SuaveKeys.Clients
{
    public partial class App : Application
    {
        public new static App Current;
        //private readonly TinyIoCContainer _container;
        public App()
        {
            //_container = container;
            InitializeComponent();
            Current = this;
            RegisterDependencies();

            // TODO: check if we have auth tokens, then set the main page
            MainPage = new LandingPage();
        }

        private void RegisterDependencies()
        {
            DependencyService.Register<IAuthService, AuthService>();
        }

        public void SetMainPage()
        {
            MainPage = new NavigationPage(new MainPage());
        }

        public void SetLandingPage()
        {
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
