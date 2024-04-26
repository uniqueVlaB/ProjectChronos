using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.Services
{
    public class WorkService
    {

        public WorkService() { }

        public Task<bool> StartDailyWork()
        {
#if ANDROID
                if(Preferences.Get("DailyWorkInProcess", bool.FalseString) == bool.TrueString)return Task.FromResult(true);
                    var workManager = AndroidX.Work.WorkManager.GetInstance(Android.App.Application.Context);
                    AndroidX.Work.PeriodicWorkRequest workRequest = AndroidX.Work.PeriodicWorkRequest.Builder.From<ProjectChronos.Droid.DailyBackgroundWork>(TimeSpan.FromDays(1)).Build();
                    workManager.Enqueue(workRequest);
                    AndroidX.Work.WorkInfo workInfo = (AndroidX.Work.WorkInfo)workManager.GetWorkInfoById(workRequest.Id).Get();
                    AndroidX.Work.WorkInfo.State workState = workInfo.GetState();
                       // if(workState == AndroidX.Work.WorkInfo.State.Succeeded) 
                      //  {
                            Preferences.Set("AndroidDailyWorkId", workRequest.Id.ToString());
                            Preferences.Set("DailyWorkInProcess", bool.TrueString);
                            Task.FromResult(true);
                      //  }
#endif

            return Task.FromResult(false);
        }

        public Task<bool> StopDailyWork()
        {
#if ANDROID
            if(!Preferences.Get("DailyWorkInProcess", false)) 
                return Task.FromResult(true);  

            var WManager = AndroidX.Work.WorkManager.GetInstance(Android.App.Application.Context);
            string RawIdStr = Preferences.Get("AndroidDailyWorkId", string.Empty);

            if(RawIdStr == string.Empty) 
                return Task.FromResult(true);

            Java.Util.UUID WorkId = Java.Util.UUID.FromString(RawIdStr);
            WManager.CancelWorkById(WorkId);

             Preferences.Set("AndroidDailyWorkId", string.Empty);
             Preferences.Set("DailyWorkInProcess", bool.FalseString);
#endif

            return Task.FromResult(true);
        }
    }
}
