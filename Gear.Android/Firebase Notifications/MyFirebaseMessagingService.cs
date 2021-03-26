
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using Gear.Droid;

[Service]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class MyFirebaseMessagingService : FirebaseMessagingService
{
    // private string TAG = "MyFirebaseMsgService";
   
    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);
        try
        {
            SendNotification(message.GetNotification().Body, message.GetNotification().Title, message.Data);
            
        }
        catch (Exception ex)
        {
        }

    }
    private void SendNotification(string messageBody, string messageTitle, IDictionary<string, string> data)
    {

        var intent = new Intent(this, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.ClearTop);
        foreach (var key in data.Keys)
        {
            intent.PutExtra(key, data[key]);
        }
        var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);
        var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID).SetSmallIcon(Resource.Drawable.icon_notification).SetContentTitle(messageTitle).SetContentText(messageBody).SetAutoCancel(true).SetContentIntent(pendingIntent).SetVibrate(new long[] { 1000, 1000 }).SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification)).SetStyle((new NotificationCompat.BigTextStyle().BigText(messageBody)));
        var notificationManager = NotificationManagerCompat.From(this); notificationManager.Notify(new Random().Next(), notificationBuilder.Build());
    }
}