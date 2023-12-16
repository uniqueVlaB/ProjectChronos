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

}

