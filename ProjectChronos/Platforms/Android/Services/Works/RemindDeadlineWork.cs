using Android.Content;
using AndroidX.Work;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using ProjectChronos.Models.App;
using ProjectChronos.Services;


namespace ProjectChronos.Platforms.Android.Services.Works
{
    public class RemindDeadlineWork : Worker
    {
        public RemindDeadlineWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            var storageService = new StorageService();
            var deadlines = storageService.GetDeadlines();
            GenerateNextDayDeadlineNotifications(deadlines);
            GenerateInOneHourDeadlineNotifications(deadlines);
            return Result.InvokeSuccess();
        }

        void GenerateInOneHourDeadlineNotifications(List<DeadlineInfo> deadlines)
        {
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
                        NotificationId = (deadline.DeadlineTime.Day + deadline.DeadlineTime.Month + deadline.DeadlineTime.Year + deadline.DeadlineTime.Hour + deadline.DeadlineTime.Minute) / 24,
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
    }
}
