
using ProjectChronos.ViewModels;

namespace ProjectChronos.Pages;

public partial class AddDeadlinePage : ContentPage
{
	public AddDeadlinePage()
	{
		InitializeComponent();
    }
    public AddDeadlinePage(DeadlinesPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}