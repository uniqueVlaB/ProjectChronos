using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.LocalNotification;
using ProjectChronos.View.PopUPs;
using ProjectChronos.ViewModel;

namespace ProjectChronos;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS) {
        NotificationRequest miRequest = new NotificationRequest
        {
            NotificationId = Random.Shared.Next(),
            Title = "test work",
            BadgeNumber = 42,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now
            },
        };

        LocalNotificationCenter.Current.Show(miRequest);
        }
    }

}

