using System.Windows.Input;

namespace ProjectChronos.Views.Widgets;

public partial class ToggleParameterWidget : Grid
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(ToggleParameterWidget),
        default(string));

    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
        nameof(IsToggled),
        typeof(bool),
        typeof(ToggleParameterWidget),
        default(bool),
        BindingMode.TwoWay);

    public string Title
    { 
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value); 
    }

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    public static readonly BindableProperty ToggledCommandProperty =
        BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(ToggleParameterWidget), null);

    public ICommand ToggledCommand
    {
        get => (ICommand)GetValue(ToggledCommandProperty);
        set => SetValue(ToggledCommandProperty, value);
    }

    public ToggleParameterWidget()
    {
        InitializeComponent();
    }

    
}
