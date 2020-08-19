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
    public partial class CommandLogPage : ContentPage
    {
        private readonly CommandLogPageViewModel _vm = new CommandLogPageViewModel();
        public CommandLogPage()
        {
            InitializeComponent();
            BindingContext = _vm;
            _vm.CommandItems.CollectionChanged += CommandItems_CollectionChanged;
        }

        private void CommandItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CommandLogView.ScrollTo(_vm.CommandItems.Count - 1);
            });
        }
    }
}