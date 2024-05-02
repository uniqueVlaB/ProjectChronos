using ProjectChronos.Models.App.Deadlines;

namespace ProjectChronos.Views.Widgets;

public partial class DeadlineCardWidget : SwipeView
{
    public static readonly BindableProperty DeadlineInfoProperty = BindableProperty.Create(
        nameof(DeadlineInfo),
        typeof(DeadlineInfo),
        typeof(DeadlineCardWidget),
        default(DeadlineInfo));
    
    public DeadlineInfo DeadlineInfo
    {
        get => (DeadlineInfo)GetValue(DeadlineInfoProperty);
        set => SetValue(DeadlineInfoProperty, value);
    }
    public DeadlineCardWidget()
	{
		InitializeComponent();
	}
}