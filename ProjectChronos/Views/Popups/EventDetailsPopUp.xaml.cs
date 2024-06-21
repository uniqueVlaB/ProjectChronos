using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectChronos.Models.App;

namespace ProjectChronos.Views.Popups;

public partial class EventDetailsPopUp
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