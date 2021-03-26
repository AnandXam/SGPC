using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase.Messaging;
using Xamarin.Forms;

namespace Gear.Droid
{
    [Activity(Label = "SGPC", Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "sgpcnotificationchannel";
        internal static readonly string FOREGROUND_NOTIFICATION_CHANNEL_ID = "foreground_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            MessagingCenter.Subscribe<object>(this, "SubScribeFCM", (sender) =>
            {
                FirebaseMessaging.Instance.SubscribeToTopic("all");
            });
            MessagingCenter.Subscribe<object>(this, "UnSubScribeFCM", (sender) =>
            {
                FirebaseMessaging.Instance.UnsubscribeFromTopic("all");
            });

            CreateNotificationChannel();
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 25, 35, 37));
            if (Device.Idiom == TargetIdiom.Phone)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
            else
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        #region Create Notification channel

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }
            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.High)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };
            channel.EnableVibration(true);
            channel.EnableLights(true);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        #endregion
    }
}