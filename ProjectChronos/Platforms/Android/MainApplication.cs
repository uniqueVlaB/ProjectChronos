using Android;
using Android.App;
using Android.OS;
using Android.Runtime;

[assembly: UsesPermission(Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]
[assembly: UsesPermission(Manifest.Permission.Vibrate)]
[assembly: UsesPermission("android.permission.POST_NOTIFICATIONS")]
[assembly: UsesPermission("android.permission.USE_EXACT_ALARM")]
[assembly: UsesPermission("android.permission.SCHEDULE_EXACT_ALARM")]
namespace ProjectChronos;

[Application]
public class MainApplication : MauiApplication
{

    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
   
   
}
