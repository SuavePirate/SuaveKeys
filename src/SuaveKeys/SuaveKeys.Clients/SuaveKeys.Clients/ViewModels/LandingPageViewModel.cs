using Acr.UserDialogs;
using SuaveKeys.Clients.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class LandingPageViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        public ICommand SignInCommand { get; set; }
        public LandingPageViewModel()
        {
            _authService = App.Current.Container.Resolve<IAuthService>();

            Title = "Welcome to Suave Keys";
            SignInCommand = new Command(async () =>
            {
                var result = await _authService.StartAuthentication();
                if (result?.ResultType == ServiceResult.ResultType.Ok)
                    App.Current.SetMainPage();
            });
        }
    }
}
