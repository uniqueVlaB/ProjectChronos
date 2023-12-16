using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectChronos.Model.App;

namespace ProjectChronos.View.PopUPs;

public partial class EventDetailsPopUp : Popup
{

     public EventInfo eventInfo { get; set; }
	public EventDetailsPopUp()
	{
		InitializeComponent();
	}
    public EventDetailsPopUp(EventInfo eventInfo)
    {
        this.eventInfo = eventInfo;
        BindingContext = this;
        InitializeComponent();
    }
}