using Android.Content;
using AndroidX.Work;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using ProjectChronos.Models.App;
using ProjectChronos.Models.Cist.Events;
using ProjectChronos.Services;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectChronos.Platforms.Android.Services.Works
{
    public class RemindPairWork : Worker
    {
        public RemindPairWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            var storageService = new StorageService();
            var timetable = storageService.GetTimetable();
            GeneratePairStartsNotifications(timetable);
            return Result.InvokeSuccess();
        }

        void GeneratePairStartsNotifications(Timetable timetable)
        {
            var result = string.Empty;
            foreach (var Event in timetable.Events)
            {
                if ((Event.StartTime.Date == DateTime.Today && Event.StartTime > DateTime.Now) || Event.StartTime.Date == DateTime.Today.AddDays(1))
                {
                    var lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId));

                    NotificationRequest in15min = new NotificationRequest
                    {
                        NotificationId = (int)(Event.LessonId / 2 + 15),
                        Title = "Pair is in 15 minutes",
                        Description = $"{lesson.FullName} will start in 15 minutes",
                        Subtitle = lesson.ShortName,
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = Event.StartTime.AddMinutes(-15)
                        },
                    };
                    LocalNotificationCenter.Current.Show(in15min);
                    NotificationRequest ontime = new NotificationRequest
                    {
                        NotificationId = (int)(Event.LessonId / 2),
                        Title = "Pair has been started",
                        Description = $"{lesson.FullName} has been started",
                        Subtitle = lesson.ShortName,
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = Event.StartTime,
                        },
                    };
                    LocalNotificationCenter.Current.Show(ontime);
                    result += lesson.FullName + "->" + Event.StartTime.ToString("dd.MM.yyyy HH:mm") + "\n";
                }
            }
        }
    }
}
