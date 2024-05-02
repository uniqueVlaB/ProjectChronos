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

            if (Preferences.Get("DailyWorkInProcess", bool.FalseString) == bool.TrueString) return Task.FromResult(true);
            var workManager = WorkManager.GetInstance(Platform.AppContext);
            PeriodicWorkRequest workRequest = PeriodicWorkRequest.Builder.From<DailyBackgroundWork>(TimeSpan.FromDays(1)).AddTag("DailyBackgroundWork").Build();
            workManager.Enqueue(workRequest);
            //WorkInfo workInfo = (WorkInfo)workManager.GetWorkInfoById(workRequest.Id).Get();
            //WorkInfo.State workState = workInfo.GetState();
            // if(workState == AndroidX.Work.WorkInfo.State.Succeeded) 
            //  {
            Preferences.Set("DailyWorkInProcess", bool.TrueString);
            //  }

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
    }
}
