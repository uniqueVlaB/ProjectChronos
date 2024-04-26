using Microsoft.Maui.Controls;
using ProjectChronos.Models.App.Deadlines;
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