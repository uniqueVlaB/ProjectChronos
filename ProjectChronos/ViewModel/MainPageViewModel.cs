
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectChronos.Model.App;
using ProjectChronos.Services;
using ProjectChronos.View.PopUPs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using ProjectChronos.Model.Cist.Events;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ProjectChronos.ViewModel
{

    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isRefreshing;
        public bool IsNotRefreshing => !IsRefreshing;
        public ObservableCollection<EventInfo> TodayEvents { get; } = new();
        public ObservableCollection<EventInfo> TomorrowEvents { get; } = new();
        public ObservableCollection<EventInfo> ThisWeekEvents { get; } = new();

        CistService cistService;
        public MainPageViewModel(CistService cistService) {
            this.cistService = cistService;
                Title = Preferences.Get("GroupName", "");
                SetEventsWithStoredTimetable();
        }

        void SetEventsWithStoredTimetable() {
            Timetable timetable = new();

            var serializer = new XmlSerializer(typeof(Timetable));
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "events.xml");

            try
            {
                using (var reader = new StreamReader(path))
                {
                    timetable = (Timetable)serializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException)
            {
                return;
            }
            catch (DirectoryNotFoundException)
            {
                return;
            }

            if (timetable is null) return;
            // var timetableStr = await SecureStorage.GetAsync("timetable");

            var todayEvents = GetEventsInfosByDateWithOffset(timetable, DateTime.Now, 0);

            if (TodayEvents.Count != 0)
                TodayEvents.Clear();

            foreach (var _event in todayEvents)
                TodayEvents.Add(_event);

            var tomorrowEvents = GetEventsInfosByDateWithOffset(timetable, DateTime.Now, 1);

            if (TomorrowEvents.Count != 0)
                TomorrowEvents.Clear();

            foreach (var _event in tomorrowEvents)
                TomorrowEvents.Add(_event);

            var thisWeekEvents = GetThisWeekEventsInfos(timetable, DateTime.Now);

            if (ThisWeekEvents.Count != 0)
                ThisWeekEvents.Clear();

            foreach (var _event in thisWeekEvents)
                ThisWeekEvents.Add(_event);
        }

        [RelayCommand]
        async Task PopUpDetails(EventInfo eventInfo)
        {
            if (eventInfo == null)
                return;

            var popup = new EventDetailsPopUp(eventInfo);

           await Shell.Current.ShowPopupAsync(popup);
           
        }
        [RelayCommand]
        async Task GetEventsAsync()
        {
            Title = Preferences.Default.Get("GroupName", "");
            if (IsBusy) return;

            try 
            {
               IsBusy = true;
                var timetable = await cistService.GetTimetableWithOffsetAsync(30);
                // TODO: find something better
                //   await SecureStorage.SetAsync("timetable", JsonConvert.SerializeObject(timetable));
                var serializer = new XmlSerializer(typeof(Timetable));
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                Directory.CreateDirectory(path);
                using (var writer = new StreamWriter(Path.Combine(path, "events.xml")))
                {
                    serializer.Serialize(writer, timetable);
                }

                var todayEvents = GetEventsInfosByDateWithOffset(timetable, DateTime.Now, 0);

                // TODO: create method that does this
                if (TodayEvents.Count != 0)
                    TodayEvents.Clear();

                foreach (var _event in todayEvents)
                    TodayEvents.Add(_event);

                var tomorrowEvents = GetEventsInfosByDateWithOffset(timetable, DateTime.Now, 1);

                if (TomorrowEvents.Count != 0)
                    TomorrowEvents.Clear();

                foreach (var _event in tomorrowEvents)
                    TomorrowEvents.Add(_event);

                var thisWeekEvents = GetThisWeekEventsInfos(timetable, DateTime.Now);

                if (ThisWeekEvents.Count != 0)
                    ThisWeekEvents.Clear();

                foreach (var _event in thisWeekEvents)
                    ThisWeekEvents.Add(_event);
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally 
            {
                
                IsBusy = false; 
               IsRefreshing = false;
            }
        }

        private List<EventInfo> GetEventsInfosByDateWithOffset(Timetable timetable, DateTime time,int offset)
        {
            var events = new List<EventInfo>();
            foreach (var Event in timetable.Events)
            {
                if (Event.StartTime.Date.Equals(time.Date.AddDays(offset)))
                {
                    var eventType = timetable.EventTypes.FirstOrDefault(et => et.Id.Equals(Event.TypeId));
                   
                    events.Add(new EventInfo
                    {
                        StartTime = Event.StartTime.ToLocalTime(),
                        EndTime = Event.EndTime.ToLocalTime(),
                        FullType = eventType.FullName,
                        ShortType = eventType.ShortName,
                        BaseTypeName = eventType.EnglishBaseName,
                        PairNumber = Event.PairNumber,
                        Lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId)),
                        Teachers = timetable.Teachers.Where(t => Event.TeacherIds.Contains(t.Id)).ToList(),
                        Groups = timetable.Groups.Where(g => Event.GroupIds.Contains(g.Id)).ToList(),
                        Color = GetEventColorByType(eventType.EnglishBaseName),
                        backgroundName = "back2.jpg"

                    });
                }
            }
            return events;
        }

        private Color GetEventColorByType(string EventType) {
            switch (EventType)
            {
                case "lecture":
                    return Colors.DarkOrange;
                case "practice":
                    return Colors.DarkGreen;
                case "laboratory":
                    return Colors.Purple;
                case "consultation":
                    return Colors.Azure;
                case "test":
                    return Colors.DarkTurquoise;
                case "exam":
                    return Colors.Firebrick;
                case "course_work":
                    return Colors.DarkKhaki;
                default:
                    return Colors.Blue;
            }
        }

        private List<EventInfo> GetThisWeekEventsInfos(Timetable timetable, DateTime time) {
            int daysToEndOfWeek = 7 - (int)time.DayOfWeek;
            var events = new List<EventInfo>();
            foreach (var Event in timetable.Events)
            {
                for (int i = 0; i < daysToEndOfWeek; i++)
                {
                    if (Event.StartTime.Date.Equals(time.Date.AddDays(i)))
                    {
                        var eventType = timetable.EventTypes.FirstOrDefault(et => et.Id.Equals(Event.TypeId));
                        events.Add(new EventInfo
                        {
                            StartTime = Event.StartTime.ToLocalTime(),
                            EndTime = Event.EndTime.ToLocalTime(),
                            FullType = eventType.FullName,
                            ShortType = eventType.ShortName,
                            BaseTypeName = eventType.EnglishBaseName,
                            PairNumber = Event.PairNumber,
                            Lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId)),
                            Teachers = timetable.Teachers.Where(t => Event.TeacherIds.Contains(t.Id)).ToList(),
                            Groups = timetable.Groups.Where(g => Event.GroupIds.Contains(g.Id)).ToList(),
                            Color = GetEventColorByType(eventType.EnglishBaseName)
                        });
                    }
                }
            }
            return events;
        }
        [RelayCommand]
        public void ShowPopup() {
            var popup = new EventDetailsPopUp();
            Shell.Current.ShowPopupAsync(popup);

        }
    }
}
