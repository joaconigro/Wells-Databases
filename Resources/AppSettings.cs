namespace Wells.Resources
{
    public class AppSettings
    {
        static bool _UpgradeRequired = false;
        static string _CurrentLanguage = "es-ES";

        private AppSettings() { }

        public static void RestoreDefaultSettings()
        {
            //Not implemented yet.
        }

        public static void UpgradeSettings()
        {
            //Not implemented yet.
        }

        public static string CurrentLanguage
        {
            get
            {
                return _CurrentLanguage;
            }
            set
            {
                _CurrentLanguage = value;
            }
        }

        public static bool UpgradeRequired => _UpgradeRequired;
    }
}
