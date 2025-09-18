using Common.Services.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Log
{
    internal class Logger : ILogger
    {
        private StreamWriter logger;
        private bool disposed = false;

        public Logger(string directory, string fileName)
        {
            logger = new StreamWriter(Path.Combine(directory, fileName), append: true)
            {
                AutoFlush = true
            };
        }

        public void Log(string _event, string message)
        {
            try
            {
                logger.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] [{_event}]: {message}");
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
            if (disposed) return;

            if (disposing)
            {
                logger?.Dispose();
                logger = null;
            }
            disposed = true;
        }
    }
}
