using Microsoft.Maui.Controls;
using ProjectChronos.ViewModels;
using UraniumUI.Pages;

namespace ProjectChronos.Pages;

public partial class DeadlinesPage : UraniumContentPage
{
    bool ActionsOpened = false;
	public DeadlinesPage(DeadlinesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
    }
}