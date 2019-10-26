namespace Wells.Resources
{
    public class AppSettings
    {
        static bool _UpgradeRequired = false;
        static string _CurrentLanguage = "es-ES";

        private AppSettings() { }

        public static void RestoreDefaultSettings()
        {

        }

        public static void UpgradeSettings()
        {

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

        public static bool UpgradeRequired => UpgradeRequired;
    }
}
