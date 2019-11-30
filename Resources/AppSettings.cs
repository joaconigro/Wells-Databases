using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wells.Base;

namespace Wells.Resources
{
    public class AppSettings
    {
        private AppSettings() { }

        public string MapCredentialsProvider { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }

        [JsonIgnore]
        public string CurrentConnectionString { get; private set; }

        [JsonIgnore]
        public string CurrentDbName { get; private set; }

        public static string SettingsDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Wells");
        
        public static AppSettings Initialize()
        {
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            var filename = Path.Combine(SettingsDirectory, "Settings.was");

            try
            {
                if (File.Exists(filename))
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var jsonString = File.ReadAllText(filename);
                    return JsonSerializer.Deserialize<AppSettings>(jsonString, options);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, false);
                return null;
            }
        }

        public void SetConnectionString(string key)
        {
            if (ConnectionStrings.ContainsKey(key))
            {
                CurrentConnectionString = ConnectionStrings[key];
                CurrentDbName = key;
            }
        }
    }
}
