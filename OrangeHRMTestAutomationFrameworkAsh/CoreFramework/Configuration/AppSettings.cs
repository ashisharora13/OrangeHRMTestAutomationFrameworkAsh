using Microsoft.Extensions.Configuration;

namespace OrangeHRM.Automation.Framework.Core.Configuration
{
    public class AppSettings
    {
        public string BaseUrl { get; set; } = "https://opensource-demo.orangehrmlive.com/";
        public string Browser { get; set; } = "Chrome";
        public string AdminUsername { get; set; } = "Admin";
        public string AdminPassword { get; set; } = "admin123";
        public int DefaultTimeout { get; set; } = 30;
    }
}


