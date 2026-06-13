using System;
using System.Windows;

namespace RDR2PhotoConverter
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Logger.LogSessionStart();
            Logger.Log("Application started");

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Logger.LogSessionEnd();
            base.OnExit(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogException("DispatcherUnhandledException", e.Exception);
            MessageBox.Show($"An unexpected error occurred.\n\nRAW: {e.Exception.Message}");
            e.Handled = true;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Logger.LogFatal("AppDomain.UnhandledException", ex);
            MessageBox.Show($"A fatal error occurred and the application needs to close.\n\nRAW: {ex?.Message}");
        }
    }
}