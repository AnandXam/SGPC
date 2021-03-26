using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Gear
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Userid.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.Userid.Unfocused += (s, e) => { SetLayoutPosition(onFocus: false); };
            this.Password.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.Password.Unfocused += (s, e) => { SetLayoutPosition(onFocus: false); };

        }

        private async void Login(object sender, EventArgs e)
        {
            try
            {
                if (Userid.Text == "admin" && Password.Text == "admin")
                {
                    Preferences.Set("isLogined","true");
                    MessagingCenter.Send<object>(this, "SubScribeFCM");
                    Application.Current.MainPage = new NavigationPage(new DashboardPage());
                }
                else
                {
                    await DisplayAlert("Invalid Credentials!", "Userid or Password is wrong.", "OK");
                }
            }
            catch (Exception) { }
        }

        #region Keyboard Focusing and unfocusing Transition
        void SetLayoutPosition(bool onFocus)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.MainLayout.TranslateTo(0, -100, 50, Easing.SinIn);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.MainLayout.TranslateTo(0, -100, 100, Easing.CubicIn);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.MainLayout.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.MainLayout.TranslateTo(0, 0, 100, Easing.CubicInOut);
                }
            }
        }
        #endregion Keyboard Focusing and unfocusing Transition

    }
}
