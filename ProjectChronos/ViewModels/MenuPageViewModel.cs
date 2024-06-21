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
using ProjectChronos.Services.PlatformSpecificInterfaces;



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
        bool pairRemindEnabled;
        [ObservableProperty]
        bool deadlineRemindEnabled;
        IWorkService workService;
   
        public MenuPageViewModel(IWorkService workService)
        {
            this.workService = workService;
            PairRemindEnabled = bool.Parse(Preferences.Get("PairRemindEnabled", bool.FalseString));
            DeadlineRemindEnabled = bool.Parse(Preferences.Get("DeadlineRemindEnabled", bool.FalseString));
        }

        [RelayCommand]
        async Task ToggleRemindPairsAsync()
        {
            if (!PairRemindEnabled)
            {
                await workService.StopPairRemindWork();
                Preferences.Set("PairRemindEnabled", bool.FalseString);
                return;
            }

            if (LocalNotificationCenter.Current.AreNotificationsEnabled().Result == false)
            {
                if (!await RequestNotificationsPermissionAsync())
                {
                    PairRemindEnabled = false;
                    return;
                }
            }
            if (PairRemindEnabled)
            {
                await workService.StartPairRemindWork();
                Preferences.Set("PairRemindEnabled", bool.TrueString);
            }
        }

        [RelayCommand]
        async Task ToggleDeadlineRemindAsync()
        {
            if (!DeadlineRemindEnabled)
            {
                await workService.StopDeadlineRemindWork();
                Preferences.Set("DeadlineRemindEnabled", bool.FalseString);
                return;
            }

            if (LocalNotificationCenter.Current.AreNotificationsEnabled().Result == false)
            {
                if (!await RequestNotificationsPermissionAsync())
                {
                    DeadlineRemindEnabled = false;
                    return;
                }
            }
            if (DeadlineRemindEnabled)
            {
                await workService.StartDeadlineRemindWork();
                Preferences.Set("DeadlineRemindEnabled", bool.TrueString);
            }
        }

        async Task<bool> RequestNotificationsPermissionAsync() {

            var userAllowedPermissionRequest = await Shell.Current.DisplayAlert($"No permisson.", "Notifications permission required for reminders." +
                                                                               "\nWould you like to allow notifications?", "Yes", "No");
            if (!userAllowedPermissionRequest)
            {
                return false;
            }

            var userAllowedNotifications = await LocalNotificationCenter.Current.RequestNotificationPermission();
            if (!userAllowedNotifications)
            {
                return false;
            }
            return true;
        }

        [RelayCommand]
        async Task GoToGroupSelectionPopupAsync() {
            var popup = new GroupSelectionPopup(new GroupSelectionPopupViewModel(new Services.CistService()));

            await MopupService.Instance.PushAsync(popup);
            SelectedGroup = Preferences.Default.Get("GroupName", "");
            
        }

    }
}
