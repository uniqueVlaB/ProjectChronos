using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectChronos.ViewModel.Popups;

namespace ProjectChronos.View.PopUPs;
public partial class GroupSelectionPopup
{
	public GroupSelectionPopup(GroupSelectionPopupViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}