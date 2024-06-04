using ProjectChronos.Views.Popups;
using ProjectChronos.ViewModels;
using Mopups.Services;
using ProjectChronos.Models.App;

namespace ProjectChronos.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void ScheduleView_Tap(object sender, DevExpress.Maui.Scheduler.SchedulerGestureEventArgs e)
    {
        if (e.AppointmentInfo == null) return;

        HapticFeedback.Perform(HapticFeedbackType.Click);
        var s = BindingContext as MainPageViewModel;

        var appointmentId = e.AppointmentInfo.Appointment.Id as int?;
        EventInfo info = new();

        if (appointmentId.HasValue)
        {
            info = s.Events.FirstOrDefault(e => e.Id == appointmentId);

            if (info == null) return;
        }
        MopupService.Instance.PushAsync(new EventDetailsPopUp(info));

    }
}

