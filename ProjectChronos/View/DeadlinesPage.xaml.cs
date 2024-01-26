using ProjectChronos.ViewModel;

namespace ProjectChronos.View;

public partial class DeadlinesPage : ContentPage
{
	short RotateDirection = -1; //-1 left, 1 right
	public DeadlinesPage(DeadlinesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

    private void FABbtn_Clicked(object sender, EventArgs e)
    {
		((Button)sender).RotateTo(90*RotateDirection);
        RotateDirection *= -1;

		//ActionBtnStack.IsVisible = r == -1 ? true : false;
		ActionBtnStack.TranslateTo(63*-RotateDirection,0);
    }
}