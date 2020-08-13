using SuaveKeys.Clients.ViewModels;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            _vm.Macros = new ObservableCollection<MacroModel>(model?.Configuration?.Macros ?? new List<MacroModel>());
            BindingContext = _vm;
        }

        private void EditMacroView_OnChange(object sender, MacroEditArgs e)
        {
            // send to VM to update it
        }
    }
}