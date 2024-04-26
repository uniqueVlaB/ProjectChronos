using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectChronos.Views.Popups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core.Views;
using ProjectChronos.ViewModels.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using Mopups.Services;
using Plugin.LocalNotification;
using ProjectChronos.Services;



namespace ProjectChronos.ViewModels
{
    public partial class MenuPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string selectedGroup = Preferences.Get("GroupName","");
        [ObservableProperty]
        public string pairBeforeString;
        [ObservableProperty]
        public string pairNowString;
        [ObservableProperty]
        bool remindEnabled;
        WorkService workService;
        //public bool RemindEnabled
        //{
        //    get { return _remindEnabled; }
        //    set
        //    {
        //        if (_remindEnabled != value)
        //        {
        //            _remindEnabled = value;
        //            OnPropertyChanged();
        //            Preferences.Set("NotificationsWorkEnabled", _remindEnabled.ToString());

        //        }
        //    }
        //}

        public MenuPageViewModel(WorkService workService)
        {
            this.workService = workService;
            RemindEnabled = bool.Parse(Preferences.Get("DailyWorkEnabled", bool.FalseString));
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
        Task ToggleRemindPairs()
        {
            if(RemindEnabled)
                workService.StartDailyWork();
            else 
                workService.StopDailyWork();

            //if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            //{
            //    await LocalNotificationCenter.Current.RequestNotificationPermission();
            //}
            Preferences.Set("DailyWorkEnabled", RemindEnabled.ToString());
            return Task.CompletedTask;
        }

        [RelayCommand]
        async Task GoToGroupSelectionPopupAsync() {
            var popup = new GroupSelectionPopup(new GroupSelectionPopupViewModel(new Services.CistService()));

            await MopupService.Instance.PushAsync(popup);
            SelectedGroup = Preferences.Default.Get("GroupName", "");
            
        }

    }
}
