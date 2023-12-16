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
        public string pairBeforeName;
        [ObservableProperty]
        public string pairBeforeTime;
        [ObservableProperty]
        public string pairName;
        [ObservableProperty]
        public string pairTime;
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
            PairBeforeName = Preferences.Get("PlannedBeforeNotifName","Not Found");
            PairBeforeTime = Preferences.Get("PlannedBeforeNotifTime", "Not Found");
            PairName = Preferences.Get("PlannedNotifName", "Not Found");
            PairTime = Preferences.Get("PlannedNotifTime", "Not Found");
        }

        [RelayCommand]
        async Task GoToGroupSelectionPopupAsync() {
            var popup = new GroupSelectionPopup(new GroupSelectionPopupViewModel(new Services.CistService()));

            await Shell.Current.ShowPopupAsync(popup);
            SelectedGroup = Preferences.Default.Get("GroupName", "");
            
        }

    }
}
