using Microsoft.Maui.Controls;
using ProjectChronos.Model.App.Deadlines;
using ProjectChronos.ViewModel;

namespace ProjectChronos.View;

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