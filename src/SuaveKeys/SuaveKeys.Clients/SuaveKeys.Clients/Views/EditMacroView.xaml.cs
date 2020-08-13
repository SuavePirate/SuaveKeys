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
    public partial class EditMacroView : ContentView
    {
        public event EventHandler<MacroEditArgs> OnChange;
        public MacroEvent MacroEvent { get; set; }
        public EditMacroView()
        {
            InitializeComponent();
        }
    }

    public class MacroEditArgs
    {
        public MacroEvent MacroEvent { get; set; }
    }
}