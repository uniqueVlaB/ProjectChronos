using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.LocalNotification;
using ProjectChronos.Views.Popups;
using ProjectChronos.ViewModels;

namespace ProjectChronos.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
    }

}

