using Microsoft.Extensions.Configuration;

namespace OrangeHRM.Automation.Framework.Core.Configuration
{
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public ConfigurationManager()
        {
            // Build configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Bind configuration to AppSettings object
            _appSettings = new AppSettings();
            _configuration.GetSection("AppSettings").Bind(_appSettings);
        }

        // Properties that match your BaseTest requirements
        public string Browser => _appSettings.Browser;
        public int DefaultTimeout => _appSettings.DefaultTimeout;
        public string BaseUrl => _appSettings.BaseUrl;
        public string AdminUsername => _appSettings.AdminUsername;
        public string AdminPassword => _appSettings.AdminPassword;
    }
}