using SuaveKeys.Clients.ViewModels;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuaveKeys.Clients.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyboardCommandPage : ContentPage
    {
        private readonly KeyboardCommandPageViewModel _vm = new KeyboardCommandPageViewModel();
        public KeyboardCommandPage(UserKeyboardProfileModel profile)
        {
            InitializeComponent();

            _vm.CurrentProfile = profile;
            BindingContext = _vm;
        }
    }
}