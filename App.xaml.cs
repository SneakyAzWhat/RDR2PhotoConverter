using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RDR2PhotoConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //TODO
            //LOG information to a file, this is a general unhandled exception that wasn't handled within my normal try/catch blocks
            MessageBox.Show($"An unexpected error occurred and the application needs to close.\n\nRAW: {e.Exception.Message}");
            e.Handled = true;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show($"A fatal error occurred.\n\nRAW: {ex?.Message}");
        }
    }
}
