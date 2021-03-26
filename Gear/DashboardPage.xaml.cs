using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Gear.helper;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Gear
{
    public partial class DashboardPage : ContentPage
    {

        DashboardData dashboardData = new DashboardData();

        public class Datum
        {
            public int id { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class DashboardData
        {
            public int status { get; set; }
            public string message { get; set; }
            public List<Datum> data { get; set; }
            public string updated_at { get; set; }
        }



        public DashboardPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await getDashoboardData(false);
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(async () =>
                {
                    await getDashoboardData(true);
                });
                return true;
            });
        }


        #region Dashboard API Call

        private async Task getDashoboardData(bool isBackground)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {

                        Device.BeginInvokeOnMainThread(async () =>
                        {

                            if (!isBackground)
                            {
                                LoadingIndicator.IsVisible = true;
                            }
                            NetworkFrame.IsVisible = false;


                        });


                        try
                        {
                            var client = new HttpClient();
                            var response = await client.GetAsync(string.Format("https://sgpc-iot.krabd.com/v1/datas/"));
                            var responseString = await response.Content.ReadAsStringAsync();
                            dashboardData = JsonConvert.DeserializeObject<DashboardData>(responseString);

                            if (dashboardData.data != null)
                            {
                                
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    NetworkFrame.IsVisible = false;
                                    LoadingIndicator.IsVisible = false;
                                    MainGrid.IsVisible = true;

                                    ComponentLabel.Text = string.IsNullOrEmpty(dashboardData.data[0].text)?"No data": dashboardData.data[0].text;
                                    ComponentCount.Text = string.IsNullOrEmpty(dashboardData.data[0].value) ? "0" : dashboardData.data[0].value;
                                    ToolsLabel.Text = string.IsNullOrEmpty(dashboardData.data[1].text) ? "No Data" : dashboardData.data[1].text;
                                    ToolsCount.Text = string.IsNullOrEmpty(dashboardData.data[1].value) ? "0" : dashboardData.data[1].value;
                                    CoolentLabel.Text = string.IsNullOrEmpty(dashboardData.data[2].text) ? "No data" : dashboardData.data[2].text;
                                    CoolentPercent.Text = string.IsNullOrEmpty(dashboardData.data[2].value) ? "0" : dashboardData.data[2].value;
                                });
                            }
                            else
                            {
                               
                                Device.BeginInvokeOnMainThread(async () =>
                                {

                                    NetworkFrame.IsVisible = false;
                                    LoadingIndicator.IsVisible = false;
                                    MainGrid.IsVisible = true;
                                    ComponentLabel.Text = "No data available!";
                                    ComponentCount.Text = "0";
                                    ToolsLabel.Text = "No data available!";
                                    ToolsCount.Text = "0";
                                    CoolentLabel.Text = "No data available!";
                                    CoolentPercent.Text = "0";
                                });
                            }
                         
                        }
                        catch (Exception ex)
                        {
                            
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                NetworkFrame.IsVisible = false;
                                LoadingIndicator.IsVisible = false;
                                MainGrid.IsVisible = true;
                                ComponentLabel.Text = "No data available!";
                                ComponentCount.Text = "0";
                                ToolsLabel.Text = "No data available!";
                                ToolsCount.Text = "0";
                                CoolentLabel.Text = "No data available!";
                                CoolentPercent.Text = "0";
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                        LoadingIndicator.IsVisible = false;
                        MainGrid.IsVisible = false;
                        NetworkFrame.IsVisible = true;
                        });
                    }
                }
                catch (Exception ex)
                {
                    
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        NetworkFrame.IsVisible = false;
                        LoadingIndicator.IsVisible = false;
                        MainGrid.IsVisible = true;
                        ComponentLabel.Text = "No data available!";
                        ComponentCount.Text = "0";
                        ToolsLabel.Text = "No data available!";
                        ToolsCount.Text = "0";
                        CoolentLabel.Text = "No data available!";
                        CoolentPercent.Text = "0";
                    });
                }
            });
        }

        public async void NetworkRetry(System.Object sender, System.EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await getDashoboardData(false);
            }
         }
        #endregion

        #region Logout


        public async void Logout(System.Object sender, System.EventArgs e)

        {
            try
            {
                var action = await DisplayAlert("Logout?", "Are you sure to logout from SGPC?", "Cancel", "Logout");
                if (!action) {
                    Preferences.Clear();
                    MessagingCenter.Send<object>(this, "UnSubScribeFCM");
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
            }
            catch (Exception) { }
        }
        #endregion
    }

}
