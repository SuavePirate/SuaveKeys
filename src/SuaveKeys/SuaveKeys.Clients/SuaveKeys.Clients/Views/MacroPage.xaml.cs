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
    public partial class MacroPage : ContentPage
    {
        private readonly MacroPageViewModel _vm = new MacroPageViewModel();
        public MacroPage(UserKeyboardProfileModel model)
        {
            InitializeComponent();
            _vm.CurrentProfile = model;
            BindingContext = _vm;
        }
    }
}