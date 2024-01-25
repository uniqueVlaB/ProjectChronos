using ProjectChronos.ViewModel;

namespace ProjectChronos.View;

public partial class DeadlinesPage : ContentPage
{
	public DeadlinesPage(DeadlinesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}