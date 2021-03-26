using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Sansation-Bold.ttf", Alias = "SA-B")]
[assembly: ExportFont("Sansation-Light.ttf", Alias = "SA-L")]
[assembly: ExportFont("Sansation-Regular.ttf", Alias = "SA-R")]
[assembly: ExportFont("fa-regular.otf", Alias = "FA-R")]
[assembly: ExportFont("fa-solid.otf", Alias = "FA-B")]
[assembly: ExportFont("fa-regular-brands.otf", Alias = "FA-BR")]

namespace Gear
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if(Preferences.Get("isLogined", string.Empty).ToString() == "true")
            {
                MainPage = new NavigationPage(new DashboardPage());

            }
            else
            {
                MainPage = new NavigationPage(new MainPage());

            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
