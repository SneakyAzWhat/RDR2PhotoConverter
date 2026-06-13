using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RDR2PhotoConverter
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "logs.txt");

        public static void LogSessionStart()
        {
            string osVersion = RuntimeInformation.OSDescription;
            string dotnetVersion = RuntimeInformation.FrameworkDescription;

            Write($"===== Session Start: {Timestamp()} =====");
            Write($"[INFO] OS: {osVersion}");
            Write($"[INFO] .NET: {dotnetVersion}");
        }

        public static void LogSessionEnd()
        {
            Write($"===== Session End: {Timestamp()} =====\n");
        }

        public static void Log(string message)
        {
            Write($"[{Timestamp()}] [INFO] {message}");
        }

        public static void LogError(string operation, string message)
        {
            Write($"[{Timestamp()}] [ERROR] Operation: {operation} | {message}");
        }

        public static void LogException(string operation, Exception ex)
        {
            Write($"[{Timestamp()}] [ERROR] Operation: {operation}");
            Write($"  Exception: {ex.GetType().Name}: {ex.Message}");
            Write($"  Stack Trace: {ex.StackTrace}");
        }

        public static void LogFatal(string operation, Exception ex)
        {
            Write($"[{Timestamp()}] [FATAL] App terminated - Operation: {operation}");
            Write($"  Exception: {ex.GetType().Name}: {ex.Message}");
            Write($"  Stack Trace: {ex.StackTrace}");
        }

        private static void Write(string line)
        {
            try
            {
                File.AppendAllText(LogPath, line + Environment.NewLine);
            }
            catch
            {
                // If logging itself fails, silently ignore — 
                // we don't want a logging failure to crash the app
            }
        }

        private static string Timestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}