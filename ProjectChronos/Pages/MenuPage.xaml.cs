using ProjectChronos.ViewModels;

namespace ProjectChronos.Pages;

public partial class MenuPage : ContentPage
{
    public MenuPage(MenuPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}