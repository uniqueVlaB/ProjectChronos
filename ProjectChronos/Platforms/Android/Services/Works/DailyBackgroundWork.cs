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
    public class DailyBackgroundWork : Worker
    {
        public DailyBackgroundWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {   
            var storageService = new StorageService();
            var cistService = new CistService();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var yearStart = DateTime
                    .Parse($"01/01/{DateTime.Now.Year} 00:00:00", new CultureInfo("fr-FR", false));
                var yearEnd = DateTime
                    .Parse($"31/12/{DateTime.Now.Year} 23:59:59", new CultureInfo("fr-FR", false));
                storageService.SaveTimetable(cistService.GetTimetableAsync(yearStart, yearEnd).Result);
            }
            return Result.InvokeSuccess();
        }

    }

}
