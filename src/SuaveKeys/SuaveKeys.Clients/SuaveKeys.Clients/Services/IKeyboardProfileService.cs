using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public interface IKeyboardProfileService
    {
        ObservableCollection<UserKeyboardProfileModel> Profiles {get;}
        UserKeyboardProfileModel CurrentRunningProfile { get; set; }
        Task LoadProfilesAsync();
        Task AddNewProfile(string name, KeyboardProfileConfiguration model);
    }
}
