using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Plugin.LocalNotification;
using ProjectChronos.Services;
using ProjectChronos.View;
using ProjectChronos.View.PopUPs;
using ProjectChronos.ViewModel;
using ProjectChronos.ViewModel.Popups;
using UraniumUI;

namespace ProjectChronos;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			
			.UseMauiApp<App>()
			.UseLocalNotification()
			.UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .UseUraniumUIBlurs()
			.ConfigureMopups()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddMaterialIconFonts();
            });


        builder.Services.AddSingleton<CistService>();
		builder.Services.AddSingleton<StorageService>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<EventDetailsPopUp>();
		builder.Services.AddSingleton<MenuPage>();
		builder.Services.AddSingleton<MenuPageViewModel>();
		builder.Services.AddTransient<GroupSelectionPopup>();
		builder.Services.AddTransient<GroupSelectionPopupViewModel>();
        builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();
		builder.Services.AddSingleton<DeadlinesPageViewModel>();
        builder.Services.AddSingleton<DeadlinesPage>();
		builder.Services.AddTransient<AddDeadlinePage>();
        builder.Services.AddSingleton<WorkService>();

        return builder.Build();
	}
}
