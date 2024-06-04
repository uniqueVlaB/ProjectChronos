
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
using System.Globalization;



namespace ProjectChronos.ViewModels
{

    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isRefreshing;
        public bool IsNotRefreshing => !IsRefreshing;
        public ObservableCollection<EventInfo> Events { get; } = new();

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

            var events = GetEventInfos(timetable);

            if (Events.Count != 0)
                Events.Clear();

            foreach (var _event in events)
                Events.Add(_event);

        }

        [RelayCommand]
        async Task PopUpDetails(DevExpress.Maui.Scheduler.SchedulerGestureEventArgs args)
        {
            
                HapticFeedback.Perform(HapticFeedbackType.Click);
                var appointmentId = args.AppointmentInfo.Appointment.Id as int?;
                EventInfo info = new();

                if (appointmentId.HasValue)
                {
                    info = Events.FirstOrDefault(e => e.Id == appointmentId.Value);

                    if (info != null) return;
                }
                await MopupService.Instance.PushAsync(new EventDetailsPopUp(info));
           
        }
        [RelayCommand]
        async Task GetEventsAsync()
        {
            var yearStart = DateTime.Parse($"01/01/{DateTime.Now.Year} 00:00:00", new CultureInfo("fr-FR", false));
            var yearEnd = DateTime.Parse($"31/12/{DateTime.Now.Year} 23:59:59", new CultureInfo("fr-FR", false));

            Title = Preferences.Default.Get("GroupName", "");
            if (IsBusy) return;

            try 
            {
               IsBusy = true;
                var timetable = await cistService.GetTimetableAsync(yearStart, yearEnd);
                // TODO: find something better
                //   await SecureStorage.SetAsync("timetable", JsonConvert.SerializeObject(timetable));
                storageService.SaveTimetable(timetable);

                var events = GetEventInfos(timetable);

                if (Events.Count != 0)
                    Events.Clear();

                foreach (var _event in events)
                {
                    if (_event is null) continue;
                    Events.Add(_event);
                }
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

        //private List<EventInfo> GetEventsInfosByDateWithOffset(Timetable timetable, DateTime start, DateTime end)
        //{
        //    var events = new List<EventInfo>();
        //    foreach (var Event in timetable.Events)
        //    {
        //        if (Event.StartTime.Date.Equals(time.Date.AddDays(offset)))
        //        {
        //            var eventType = timetable.EventTypes.FirstOrDefault(et => et.Id.Equals(Event.TypeId));

        //            events.Add(new EventInfo
        //            {
        //                StartTime = Event.StartTime,
        //                EndTime = Event.EndTime,
        //                FullType = eventType.FullName,
        //                ShortType = eventType.ShortName,
        //                BaseTypeName = eventType.EnglishBaseName,
        //                PairNumber = Event.PairNumber,
        //                Lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId)),
        //                Teachers = timetable.Teachers.Where(t => Event.TeacherIds.Contains(t.Id)).ToList(),
        //                Groups = timetable.Groups.Where(g => Event.GroupIds.Contains(g.Id)).ToList(),
        //                Color = GetEventColorByType(eventType.EnglishBaseName),
        //                backgroundName = "back2.jpg"

        //            });
        //        }
        //    }
        //    return events;
        //}

        private List<EventInfo> GetEventInfos(Timetable timetable)
        {
            var events = new List<EventInfo>();
            foreach (var Event in timetable.Events)
            {
                    var eventType = timetable.EventTypes.FirstOrDefault(et => et.Id.Equals(Event.TypeId));

                    events.Add(new EventInfo
                    {
                        Id = events.Count,
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
                        Location = Event.Room

                    });
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
