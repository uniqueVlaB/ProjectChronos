using Android.Content;
using AndroidX.Work;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using ProjectChronos.Models.App.Deadlines;
using ProjectChronos.Models.Cist.Events;
using ProjectChronos.Services;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectChronos.Platforms.Android.Services.Works
{
    public class DailyBackgroundWork : Worker
    {
        public DailyBackgroundWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {   
            sendNowNotif(5, "entered");
            var storageService = new StorageService();
            var cistService = new CistService();

            Timetable timetable = new Timetable();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var yearStart = DateTime.Parse($"01/01/{DateTime.Now.Year} 00:00:00", new CultureInfo("fr-FR", false));
                var yearEnd = DateTime.Parse($"31/12/{DateTime.Now.Year} 23:59:59", new CultureInfo("fr-FR", false));

                timetable = cistService.GetTimetableAsync(yearStart, yearEnd).Result;
                storageService.SaveTimetable(timetable);
            }
            else {
                timetable = storageService.GetTimetable();
            }
            GeneratePairStartsNotifications(timetable);

            var deadlines = storageService.GetDeadlines();
            GenerateNextDayDeadlineNotifications(deadlines);
            GenerateInOneHourDeadlineNotifications(deadlines);
            GenerateOnDeadlineNotifications(deadlines);

            return Result.InvokeSuccess();
        }

        void GenerateOnDeadlineNotifications(List<DeadlineInfo> deadlines) 
        {
            foreach (var deadline in deadlines)
            {
                if (deadline.DeadlineTime.Date == DateTime.Today)
                {
                    NotificationRequest deadlineNotif = new NotificationRequest
                    {
                        NotificationId = (deadline.DeadlineTime.Day + deadline.DeadlineTime.Month + deadline.DeadlineTime.Year + deadline.DeadlineTime.Hour + deadline.DeadlineTime.Minute)/3,
                        Title = deadline.Title,
                        Description = deadline.Description,
                        Subtitle = "Deadline",
                        BadgeNumber = 42,
                        Group = "Deadline remind",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = deadline.DeadlineTime,
                            Android = new AndroidScheduleOptions
                            {
                                AlarmType = AndroidAlarmType.ElapsedRealtimeWakeup
                            }
                        },
                        CategoryType = NotificationCategoryType.Reminder
                    };
                    LocalNotificationCenter.Current.Show(deadlineNotif);
                }
            }
        }

        void GenerateInOneHourDeadlineNotifications(List<DeadlineInfo> deadlines) {
            foreach (var deadline in deadlines)
            {
                if (deadline.DeadlineTime.Date == DateTime.Today.AddDays(1))
                {
                    NotificationRequest deadlineNotif = new NotificationRequest
                    {
                        NotificationId = deadline.DeadlineTime.Day + deadline.DeadlineTime.Month + deadline.DeadlineTime.Year + deadline.DeadlineTime.Hour + deadline.DeadlineTime.Minute,
                        Title = deadline.Title,
                        Description = deadline.Description,
                        Subtitle = "Deadline in 1 hour",
                        BadgeNumber = 42,
                        Group = "Deadline remind",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = deadline.DeadlineTime.AddHours(-1),
                            Android = new AndroidScheduleOptions
                            {
                                AlarmType = AndroidAlarmType.ElapsedRealtimeWakeup
                            }
                        },
                        CategoryType = NotificationCategoryType.Reminder
                    };
                    LocalNotificationCenter.Current.Show(deadlineNotif);
                }
            }
        }

       void GenerateNextDayDeadlineNotifications(List<DeadlineInfo> deadlines)
        {
            foreach (var deadline in deadlines)
            {
                if (deadline.DeadlineTime.Date == DateTime.Today.AddDays(2)) 
                {
                    NotificationRequest deadlineNotif = new NotificationRequest
                    {
                        NotificationId = (deadline.DeadlineTime.Day+deadline.DeadlineTime.Month+deadline.DeadlineTime.Year+deadline.DeadlineTime.Hour+deadline.DeadlineTime.Minute)/24,
                        Title = deadline.Title,
                        Description = deadline.Description,
                        Subtitle = "Deadline tommorow",
                        BadgeNumber = 42,
                        Group = "Deadline remind",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = deadline.DeadlineTime.AddDays(-1),
                             Android = new AndroidScheduleOptions 
                             {
                              AlarmType = AndroidAlarmType.ElapsedRealtimeWakeup,
                              AllowedDelay = TimeSpan.FromHours(23)
                             }
                        },
                        CategoryType = NotificationCategoryType.Reminder
                    };
                    LocalNotificationCenter.Current.Show(deadlineNotif);
                }
            }
        }

        void GeneratePairStartsNotifications(Timetable timetable) {
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
            sendNowNotif(6, result);
        }

        void sendNowNotif(int offset, string text)
        {
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
