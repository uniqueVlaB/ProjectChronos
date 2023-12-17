namespace ProjectChronos;

using Plugin.LocalNotification;
using ProjectChronos.ViewModel;

public partial class MenuPage : ContentPage
{
    public MenuPage(MenuPageViewModel viewModel)
    {

        BindingContext = viewModel;
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (DeviceInfo.Platform == DevicePlatform.Android) {
            NotificationRequest testRequest = new NotificationRequest
            {
                NotificationId = 228,
                Title = "Pair is in 15 minutes",
                Description = $"Комп'ютерні системи will start in 15 minutes",
                Subtitle = "КС",
                BadgeNumber = 42,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                },
            };
            LocalNotificationCenter.Current.Show(testRequest);
        }
    }
}