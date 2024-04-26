using Android.Content;
using AndroidX.Work;
using Plugin.LocalNotification;
using ProjectChronos.Models.Cist.Events;
using ProjectChronos.Services;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectChronos.Droid
{
    public class DailyBackgroundWork : Worker
    {
        public DailyBackgroundWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            var result = string.Empty;
            sendNowNotif(5, "entered");
            var storageService = new StorageService();

            Timetable timetable = storageService.GetTimetable();

            foreach (var Event in timetable.Events)
            {
                if (Event.StartTime.Date == DateTime.Today && Event.StartTime > DateTime.Now || Event.StartTime.Date == DateTime.Today.AddDays(1))
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
            sendNowNotif(6, result);
            return Result.InvokeSuccess();
        }

        void sendNowNotif(int offset, string text) {
            NotificationRequest miRequest = new NotificationRequest
            {
                NotificationId = Random.Shared.Next(),
                Title = "Work Stat",
                Silent = true,
                Description = text,
                BadgeNumber = 42,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(offset), 
                },
            };
            LocalNotificationCenter.Current.Show(miRequest);
        }
    }

}
