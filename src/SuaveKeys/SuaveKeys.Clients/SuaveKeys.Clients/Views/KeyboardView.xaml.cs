using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuaveKeys.Clients.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyboardView : ContentView
    {
        public static readonly BindableProperty SelectedKeyProperty = BindableProperty.Create(nameof(SelectedKey), typeof(string), typeof(KeyboardView), null);

        public string SelectedKey
        {
            get { return (string)GetValue(SelectedKeyProperty); }
            set { SetValue(SelectedKeyProperty, value); }
        }

        public KeyboardView()
        {
            InitializeComponent();
        }

        private void KeyView_OnPressed(object sender, KeyView.KeyPressedEventArgs e)
        {
            SelectedKey = e.Key;
        }
    }
}