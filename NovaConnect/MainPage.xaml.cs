using NovaConnect.Runtimes;
using System.Diagnostics;
using NovaConnect.Runtimes.Models;
using Newtonsoft.Json;
using NovaConnect.Runtimes.Models.Enum;

namespace NovaConnect
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine(Application.Current.RequestedTheme);
            ApplicationRuntimes.Theme = Application.Current.RequestedTheme;
            ApplicationRuntimes.ScreenRatioType = GetScreenType();
            ReadJsonFileAsync();

            Content = blazorWebView;
            /*
            Task.Run(() =>
            {
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    ApplicationRuntimes.IsNetworkAccess = MauiProgram.Functions.CheckNetworkAccess();
                    Debug.WriteLine($"IsNetworkAccess：{ApplicationRuntimes.IsNetworkAccess}");
                    return true;
                });

            });
             */

        }

        public void ReadJsonFileAsync()
        {
            try
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("Data/navs.json").Result;
                using var reader = new StreamReader(stream);
                var contents = reader.ReadToEnd();
                ApplicationRuntimes.NavItems = JsonConvert.DeserializeObject<NavItem[]>(contents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading file: {ex.Message}");
            }
        }
        public static ScreenRatio GetScreenType()
        {
            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

            if (screenWidth > screenHeight)
            {
                return ScreenRatio.WideScreen;
            }
            else
            {
                return ScreenRatio.NarrowScreen;
            }
        }
    }
}
