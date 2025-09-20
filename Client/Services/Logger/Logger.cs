using Common.Services.Logger;
using System;
using System.IO;

namespace Client.Services.Log
{
    internal class Logger : ILogger
    {
        private StreamWriter logger;
        private StreamWriter eventLogger;
        private StreamWriter leftoverLogger;

        private bool disposed = false;

        public Logger(string directory, string logFile, string eventFile, string leftoverFile)
        {
            logger = new StreamWriter(Path.Combine(directory, logFile))
            {
                AutoFlush = true
            };

            eventLogger = new StreamWriter(Path.Combine(directory, eventFile))
            {
                AutoFlush = true
            };

            leftoverLogger = new StreamWriter(Path.Combine(directory, leftoverFile))
            {
                AutoFlush = true
            };
        }

        public void Log(string _event, string message)
        {
            try
            {
                logger.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}] [{_event}]: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        public void LogEvent(string _event, string message)
        {
            try
            {
                eventLogger.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}] [{_event}]: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        public void LogLeftover(string _event, string message)
        {
            try
            {
                leftoverLogger.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}] [{_event}]: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        ~Logger()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("[Logger]: Disposing...");
            if (disposed) return;

            if (disposing)
            {
                if (logger != null)
                {
                    logger.Dispose();
                    logger = null;
                }

                if (eventLogger != null)
                {
                    eventLogger.Dispose();
                    eventLogger = null;
                }

                if (leftoverLogger != null)
                {
                    leftoverLogger.Dispose();
                    leftoverLogger = null;
                }

            }
            disposed = true;
        }
    }
}
