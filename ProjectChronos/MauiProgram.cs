using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using ProjectChronos.Services;
using ProjectChronos.View.PopUPs;
using ProjectChronos.ViewModel;
using ProjectChronos.ViewModel.Popups;

namespace ProjectChronos;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});


        builder.Services.AddSingleton<CistService>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<EventDetailsPopUp>();
		builder.Services.AddSingleton<MenuPage>();
		builder.Services.AddSingleton<MenuPageViewModel>();
		builder.Services.AddTransient<GroupSelectionPopup>();
		builder.Services.AddTransient<GroupSelectionPopupViewModel>();
        builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();
		builder.Services.AddSingleton<DeadlinesPageViewModel>();
        builder.Services.AddSingleton<View.DeadlinesPage>();

        return builder.Build();
	}
}
