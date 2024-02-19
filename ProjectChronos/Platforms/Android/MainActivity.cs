using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Work;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.LocalNotification;
using ProjectChronos.Droid;
using System;


namespace ProjectChronos;

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var workManager = WorkManager.GetInstance(ApplicationContext);

        if (bool.FalseString == Preferences.Get("NotificationsWorkEnabled", bool.FalseString))
        {
            LocalNotificationCenter.Current.CancelAll();
            workManager.CancelAllWork();
            Preferences.Remove("NotificationWorkId");
            Preferences.Set("NotificationsWorkInProcess", bool.FalseString);
        }
        else
        {
            if (bool.FalseString == Preferences.Get("NotificationsWorkInProcess", bool.FalseString))
            {
                PeriodicWorkRequest WorkRequest = PeriodicWorkRequest.Builder.From<PeriodicBackgroundWork>(TimeSpan.FromMinutes(30)).Build();
                workManager.Enqueue(WorkRequest);
                Preferences.Set("NotificationWorkId", WorkRequest.Id.ToString());
                Preferences.Set("NotificationsWorkInProcess", bool.TrueString);
            }
        }

    }

    //protected override void OnDestroy()
    //{

    //    var workManager = WorkManager.GetInstance(ApplicationContext);

    //    if (bool.FalseString == Preferences.Get("NotificationsWorkEnabled", bool.FalseString))
    //    {
    //        workManager.CancelAllWork();
    //        Preferences.Remove("NotificationWorkId");
    //        Preferences.Set("NotificationsWorkInProcess", bool.FalseString);
    //    }
    //    else
    //    {
    //        if (bool.FalseString == Preferences.Get("NotificationsWorkInProcess", bool.FalseString))
    //        {
    //            PeriodicWorkRequest WorkRequest = PeriodicWorkRequest.Builder.From<PeriodicBackgroundWork>(TimeSpan.FromMinutes(30)).Build();
    //            workManager.Enqueue(WorkRequest);
    //            Preferences.Set("NotificationWorkId", WorkRequest.Id.ToString());
    //            Preferences.Set("NotificationsWorkInProcess", bool.TrueString);
    //        }
    //    }
    //    base.OnDestroy();
    //}

}

