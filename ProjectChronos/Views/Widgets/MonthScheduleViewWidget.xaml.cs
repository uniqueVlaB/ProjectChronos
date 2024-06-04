using ProjectChronos.Graphics;
using ProjectChronos.Models.App;
using System.Collections.ObjectModel;
using static Android.Provider.CalendarContract;

namespace ProjectChronos.Views.Widgets;

public partial class MonthScheduleViewWidget : CarouselView
{
    public static readonly BindableProperty EventsProperty = BindableProperty.Create(
      nameof(Events),
      typeof(List<EventInfo>),
      typeof(MonthScheduleViewWidget),
      default(List<EventInfo>));

    public List<EventInfo> Events
    {
        get => (List<EventInfo>)GetValue(EventsProperty);
        set => SetValue(EventsProperty, value);
    }

    public ObservableCollection<MonthScheduleDrawing> MonthDrawables { get; } = new();
	public MonthScheduleViewWidget()
	{
		InitializeComponent();
        for (int i = 0; i < 12; i++) {
            MonthDrawables.Add(new MonthScheduleDrawing {page = i+1 });
        }
	}
}