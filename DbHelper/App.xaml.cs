using System;
using System.Diagnostics;
using System.Windows;
using Wells.Base;
using Wells.BaseView;
using Wells.DbHelper;
using Wells.Resources;

namespace DbHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Settings = AppSettings.Initialize();

                if (Settings != null)
                {
                    var mainWindow = new MainWindow();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Current.MainWindow = mainWindow;
                    MainWindow.Show();
                }
                else
                {
                    throw new ArgumentNullException("No se pudo leer el archivo de configuración.");
                }
            }
            catch (Exception ex)
            {
                SharedBaseView.ShowErrorMessageBox(new WaitingView(""), ex.Message);
                ExceptionHandler.Log(ex, TraceEventType.Critical, string.Empty);
                Current.Shutdown(-1);
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                SharedBaseView.ShowErrorMessageBox(new WaitingView(""), "Application will shut down because irrecoverable error. More info: " + e.ExceptionObject.ToString());
            }
            else
            {
                SharedBaseView.ShowErrorMessageBox(new WaitingView(""), e.ExceptionObject.ToString());
            }
            ExceptionHandler.Log(e.ExceptionObject.ToString(), TraceEventType.Critical, "Unhandled Exception");
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
        }
    }
}
