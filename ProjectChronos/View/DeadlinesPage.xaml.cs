using Microsoft.Maui.Controls;
using ProjectChronos.ViewModel;
using UraniumUI.Pages;

namespace ProjectChronos.View;

public partial class DeadlinesPage : UraniumContentPage
{
    bool ActionsOpened = false;
	public DeadlinesPage(DeadlinesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
    }

    private void FABbtn_Clicked(object sender, EventArgs e)
    {
       // ChangeStateOfActions();
    }

    //private void ScreenFiller_Tapped(object sender, TappedEventArgs e)
    //{
    //    if (ScreenFiller.IsVisible) ChangeStateOfActions();
    //}

    //private void ChangeStateOfActions()
    //{
    //    ActionsOpened = !ActionsOpened;
    //    FABbtn.RotateTo(ActionsOpened ? -135 : 0);
    //    FABbtn.BackgroundColor = ActionsOpened ? Colors.Red : Colors.Green;
    //    //ActionBtnStack.IsVisible = r == -1 ? true : false;
    //    ActionBtnStack.TranslateTo(ActionsOpened ? -211 : 211, 0);
    //    //DeadlinesContainer.IsEnabled = !ActionsOpened;
    //    ScreenFiller.IsVisible = ActionsOpened;
    //}
}