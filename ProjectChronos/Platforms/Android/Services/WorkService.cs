using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectChronos.Platforms.Android.Services.Works;
using AndroidX.Work;
using ProjectChronos.Services.PlatformSpecificInterfaces;

namespace ProjectChronos.Platforms.Android.Services
{
    public class WorkService: IWorkService
    {

        public WorkService() { }

        public Task<bool> StartDailyWork()
        {
            if (Preferences.Get("DailyWorkInProcess", bool.FalseString) == bool.TrueString)
                return Task.FromResult(true);
            var workManager = WorkManager.GetInstance(Platform.AppContext);
            PeriodicWorkRequest workRequest = PeriodicWorkRequest.Builder
                .From<DailyBackgroundWork>(TimeSpan.FromDays(1))
                .AddTag("DailyBackgroundWork").Build();
            workManager.Enqueue(workRequest);
            Preferences.Set("DailyWorkInProcess", bool.TrueString);
            return Task.FromResult(true);
        }

        public Task<bool> StopDailyWork()
        {
            if (Preferences.Get("DailyWorkInProcess", bool.FalseString) == bool.FalseString)
                return Task.FromResult(true);
            var WManager = WorkManager.GetInstance(Platform.AppContext);
            WManager.CancelAllWorkByTag("DailyBackgroundWork");
            Preferences.Set("DailyWorkInProcess", bool.FalseString);
            return Task.FromResult(true);
        }
        public Task<bool> StartPairRemindWork() 
        {
            if (Preferences.Get("PairRemindWorkInProcess", bool.FalseString) == bool.TrueString)
                return Task.FromResult(true);
            var workManager = WorkManager.GetInstance(Platform.AppContext);
            PeriodicWorkRequest workRequest = PeriodicWorkRequest.Builder
                .From<RemindPairWork>(TimeSpan.FromHours(6))
                .AddTag("PairRemindWork").Build();
            workManager.Enqueue(workRequest);
            Preferences.Set("PairRemindWorkInProcess", bool.TrueString);
            return Task.FromResult(true);
        }
        public Task<bool> StopPairRemindWork() 
        {
            if (Preferences.Get("PairRemindWorkInProcess", bool.FalseString) == bool.FalseString)
                return Task.FromResult(true);
            var WManager = WorkManager.GetInstance(Platform.AppContext);
            WManager.CancelAllWorkByTag("PairRemindWork");
            Preferences.Set("PairRemindWorkInProcess", bool.FalseString);
            return Task.FromResult(true);
        }
        public Task<bool> StartDeadlineRemindWork() 
        {
            if (Preferences.Get("DeadlineRemindWorkInProcess", bool.FalseString) == bool.TrueString)
                return Task.FromResult(true);
            var workManager = WorkManager.GetInstance(Platform.AppContext);
            PeriodicWorkRequest workRequest = PeriodicWorkRequest.Builder
                .From<RemindDeadlineWork>(TimeSpan.FromHours(6))
                .AddTag("DeadlineRemindWork").Build();
            workManager.Enqueue(workRequest);
            Preferences.Set("DeadlineRemindWorkInProcess", bool.TrueString);
            return Task.FromResult(true);
        }
        public Task<bool> StopDeadlineRemindWork() 
        {
            if (Preferences.Get("DeadlineRemindWorkInProcess", bool.FalseString) == bool.FalseString)
                return Task.FromResult(true);
            var WManager = WorkManager.GetInstance(Platform.AppContext);
            WManager.CancelAllWorkByTag("DeadlineRemindWork");
            Preferences.Set("DeadlineRemindWorkInProcess", bool.FalseString);
            return Task.FromResult(true);
        }
    }
}
