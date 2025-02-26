using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using NovaConnect.Runtimes.Models;
using NovaConnect.Runtimes.Models.Net;
using NovaConnect.Runtimes.Models.Enum;

namespace NovaConnect.Runtimes
{
    public class ApplicationRuntimes
    {
        public static class Connect
        {
            public static bool IsConnect = false;
            public static string SessionID = "";
            public static string IP = "";
            public static string Port = "";
        }
        public static ApiClient apiClient { get; set; } = new("http://novapp.w1.luyouxia.net/api");//http://192.168.0.105:5000/api

        public static ScreenRatio ScreenRatioType { get; set; }
        public static bool IsNetworkAccess { get; set; } = false;
        public static bool Loaded { get; set; } = false;
        public static object? Theme { get; set; } = "Light";
        public static bool IsInti { get; set; } = false;
        public static NavItem[] NavItems { get; set; } = new NavItem[]{};
    }
}
