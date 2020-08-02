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
    public partial class KeyView : ContentView
    {
        public string Label { get; set; }
        public bool IsPressed { get; set; }
        public event EventHandler<KeyPressedEventArgs> OnPressed;
        public int KeyWidth { get; set; } = 60;
        public KeyView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public class KeyPressedEventArgs
        {
            public string Key { get; set; }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnPressed?.Invoke(this, new KeyPressedEventArgs
            {
                Key = Label
            });
        }
    }
}