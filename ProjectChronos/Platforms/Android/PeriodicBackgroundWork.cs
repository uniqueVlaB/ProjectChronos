using Android.Content;
using AndroidX.Work;
using Plugin.LocalNotification;
using ProjectChronos.Model.Cist.Events;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectChronos.Droid
{
    public class PeriodicBackgroundWork : Worker
    {
        public PeriodicBackgroundWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            sendNowNotif(5, "DoWork entered");
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
                return Result.InvokeSuccess();
            }
            catch (DirectoryNotFoundException)
            {
                return Result.InvokeSuccess();
            }

            foreach (var Event in timetable.Events)
            {
                if (Event.StartTime.ToLocalTime() >= DateTime.Now 
                    && Event.StartTime.ToLocalTime() <= DateTime.Now.AddHours(1) 
                    && (Event.StartTime.ToLocalTime() - DateTime.Now).TotalMinutes >= 15)
                {
                    if (Preferences.Get("isBeforeNotificationSetted", bool.FalseString) == bool.FalseString)
                    {
                        var lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId));
                        Preferences.Set("PlannedBeforeNotifName", lesson.ShortName);
                        Preferences.Set("PlannedBeforeNotifTime", Event.StartTime.AddMinutes(-15).ToLocalTime().ToString("dd.MM.yyyy HH:mm"));

                        NotificationRequest miRequest = new NotificationRequest
                        {
                            NotificationId = (int)(Event.LessonId/2 + 15),
                            Title = "Pair is in 15 minutes",
                            Description =  $"{lesson.FullName} will start in 15 minutes",
                            Subtitle = lesson.ShortName,
                            BadgeNumber = 42,
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = Event.StartTime.AddMinutes(-15).ToLocalTime(),
                            },
                        };
                        LocalNotificationCenter.Current.Show(miRequest).ContinueWith(t => {
                            Preferences.Set("isBeforeNotificationSetted", bool.FalseString);
                            Preferences.Set("PlannedBeforeNotifName", bool.FalseString);
                            Preferences.Set("PlannedBeforeNotifTime", bool.FalseString);
                        });
                        Preferences.Set("isBeforeNotificationSetted", bool.TrueString);
                    }
                }

                if (Event.StartTime.ToLocalTime() >= DateTime.Now && Event.StartTime.ToLocalTime() <= DateTime.Now.AddHours(1))
                {
                    if (Preferences.Get("isNotificationSetted", bool.FalseString) == bool.FalseString)
                    {
                        var lesson = timetable.Lessons.FirstOrDefault(l => l.Id.Equals(Event.LessonId));
                        Preferences.Set("PlannedNotifName", lesson.ShortName);
                        Preferences.Set("PlannedNotifTime", Event.StartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"));

                        NotificationRequest miRequest = new NotificationRequest
                        {
                            NotificationId = (int)(Event.LessonId/2),
                            Title = "Pair has been started",
                            Description = $"{lesson.FullName} has been started",
                            Subtitle = lesson.ShortName,
                            BadgeNumber = 42,
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = Event.StartTime.ToLocalTime(),
                            },
                        };
                        LocalNotificationCenter.Current.Show(miRequest).ContinueWith(t => {
                            Preferences.Set("isNotificationSetted", bool.FalseString);
                            Preferences.Set("PlannedNotifName", bool.FalseString);
                            Preferences.Set("PlannedNotifTime", bool.FalseString);
                        });
                        Preferences.Set("isNotificationSetted", bool.TrueString);
                    }
                    
                }
            }
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
