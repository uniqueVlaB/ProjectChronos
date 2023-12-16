namespace ProjectChronos;
using ProjectChronos.ViewModel;

public partial class MenuPage : ContentPage
{
    public MenuPage(MenuPageViewModel viewModel)
    {

        BindingContext = viewModel;
        InitializeComponent();
    }
}