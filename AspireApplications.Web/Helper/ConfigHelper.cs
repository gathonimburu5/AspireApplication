namespace AspireApplications.Web.Helper
{
    public class ConfigHelper
    {
        private static ConfigHelper appSettings;
        public string appSettingValue { get; set; }
        public static string AppSetting(string key)
        {
            appSettings = GetCurrentSettings(key);
            return appSettings.appSettingValue;
        }
        public ConfigHelper(IConfiguration config, string key)
        {
            this.appSettingValue = config.GetValue<string>(key);
        }
        private static ConfigHelper GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
            IConfiguration config = builder.Build();
            var settings = new ConfigHelper(config.GetSection("ConnifigurationSettings"), key);
            return settings;
        }
    }
}
