using SuaveKeys.Clients.ViewModels;
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
    public partial class KeyboardPage : ContentPage
    {
        private readonly KeyboardPageViewModel _vm = new KeyboardPageViewModel();
        public KeyboardPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }
    }
}