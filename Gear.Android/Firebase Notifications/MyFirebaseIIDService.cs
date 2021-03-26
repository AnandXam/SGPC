using System;
using Android.App;
using Firebase.Iid;
using Xamarin.Essentials;

namespace Gear.Droid.FirebaseNotifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        [Obsolete]
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            //Preferences.Set("firebaseToken", refreshedToken);
            SecureStorage.SetAsync("firebaseToken", refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed. 
        }
    }
}
