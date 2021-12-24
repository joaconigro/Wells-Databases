using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Wells.Base;

namespace Wells.Resources
{
    public class AppSettings
    {
        public string MapCredentialsProvider { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public List<string> DbNames { get; set; }

        public string CurrentDbName { get; set; }

        public static string SettingsDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WellManager");

        public static AppSettings Initialize()
        {
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            var filename = Path.Combine(SettingsDirectory, "AppSettings.was");

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

        void Save()
        {
            var filename = Path.Combine(SettingsDirectory, "AppSettings.was");
            try
            {
                if (File.Exists(filename))
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var jsonString = JsonSerializer.Serialize(this, options);
                    File.WriteAllText(filename, jsonString);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, false);
            }
        }

        public void SetCurrentDb(string key)
        {
            CurrentDbName = key;
            Save();
        }
    }
}
