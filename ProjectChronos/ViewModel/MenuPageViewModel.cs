using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectChronos.View.PopUPs;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core.Views;
using ProjectChronos.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProjectChronos.ViewModel
{
    public partial class MenuPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string selectedGroup = Preferences.Get("GroupName","");
        private bool _remindEnabled;
        [ObservableProperty]
        public string pairBeforeString;
        [ObservableProperty]
        public string pairNowString;
        public bool RemindEnabled
        {
            get { return _remindEnabled; }
            set
            {
                if (_remindEnabled != value)
                {
                    _remindEnabled = value;
                    OnPropertyChanged();
                    Preferences.Set("NotificationsWorkEnabled", _remindEnabled.ToString());
                }
            }
        }



        public MenuPageViewModel()
        {
            _remindEnabled = bool.Parse(Preferences.Get("NotificationsWorkEnabled", bool.FalseString));
           var PairBeforeName = Preferences.Get("PlannedBeforeNotifName",bool.FalseString);
           var PairBeforeTime = Preferences.Get("PlannedBeforeNotifTime", bool.FalseString);
           var PairName = Preferences.Get("PlannedNotifName", bool.FalseString);
           var PairTime = Preferences.Get("PlannedNotifTime", bool.FalseString);
            if (PairBeforeName == bool.FalseString || PairBeforeTime == bool.FalseString)
            {
                PairBeforeString = "Unknown";
            }
            else 
            {
                PairBeforeString = $"Pair {PairBeforeName} planned to {PairBeforeTime}";
            }

            if (PairName == bool.FalseString || PairTime == bool.FalseString)
            {
                PairNowString = "Unknown";
            }
            else
            {
                PairNowString = $"Pair {PairName} planned to {PairTime}";
            }

        }

        [RelayCommand]
        async Task GoToGroupSelectionPopupAsync() {
            var popup = new GroupSelectionPopup(new GroupSelectionPopupViewModel(new Services.CistService()));

            await Shell.Current.ShowPopupAsync(popup);
            SelectedGroup = Preferences.Default.Get("GroupName", "");
            
        }

    }
}
