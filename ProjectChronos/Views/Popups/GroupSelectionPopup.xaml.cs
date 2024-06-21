using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectChronos.ViewModels.Popups;

namespace ProjectChronos.Views.Popups;
public partial class GroupSelectionPopup
{
	public GroupSelectionPopup(GroupSelectionPopupViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}