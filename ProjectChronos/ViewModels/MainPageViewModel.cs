
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectChronos.Models.App;
using ProjectChronos.Services;
using ProjectChronos.Views.Popups;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using ProjectChronos.Models.Cist.Events;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Mopups.Services;


namespace ProjectChronos.ViewModels
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
        StorageService storageService;
        public MainPageViewModel(CistService cistService, StorageService storageService) {
            this.cistService = cistService;
            this.storageService = storageService;
                Title = Preferences.Get("GroupName", "");
                SetEventsWithStoredTimetable();
        }

        void SetEventsWithStoredTimetable() {
            var timetable = storageService.GetTimetable();

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
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var popup = new EventDetailsPopUp(eventInfo);
            await MopupService.Instance.PushAsync(popup);
           
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
                storageService.SaveTimetable(timetable);

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
                        StartTime = Event.StartTime,
                        EndTime = Event.EndTime,
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
                    return Colors.LightSeaGreen;
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
                            StartTime = Event.StartTime,
                            EndTime = Event.EndTime,
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
        //[RelayCommand]
        //public async Task ShowPopup() {
        //    var popup = new EventDetailsPopUp();
        //    await MopupService.Instance.PushAsync(popup);

        //}
    }
}
