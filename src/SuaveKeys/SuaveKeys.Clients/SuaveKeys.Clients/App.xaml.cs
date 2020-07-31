using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SuaveKeys.Clients.Views;
using SuaveKeys.Clients.Services;
using TinyIoC;

namespace SuaveKeys.Clients
{
    public partial class App : Application
    {
        public new static App Current;
        private readonly TinyIoCContainer _container;
        public IKeyboardService KeyboardService;
        public App(TinyIoCContainer container)
        {
            _container = container;
            InitializeComponent();
            Current = this;
            RegisterDependencies();

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


        protected override async void OnStart()
        {
            var authService = DependencyService.Get<IAuthService>();
            var tokenResult = await authService.GetCurrentAccessToken();
            if (tokenResult?.ResultType == ServiceResult.ResultType.Ok && !string.IsNullOrEmpty(tokenResult?.Data))
                SetMainPage();

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
