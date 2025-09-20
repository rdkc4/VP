using Client.Exceptions;
using Common.Services.Data;
using System;
using System.IO;

namespace Client.Services.Data
{
    internal class DataReader : IDataReader
    {
        private StreamReader dataReader;
        private bool disposed = false;

        public DataReader(string path)
        {
            dataReader = new StreamReader(path);
        }

        public string ReadSample()
        {
            try
            {
                if (dataReader == null)
                {
                    throw new ObjectDisposedException("Data reader was disposed");
                }

                return dataReader.ReadLine();
            }
            catch (Exception ex)
            {
                throw new DataReaderException($"Error reading data: {ex.Message}");
            }
        }

        ~DataReader()
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
            Console.WriteLine("[Data Reader]: Disposing...");

            if (disposed) return;

            if (disposing)
            {
                dataReader?.Dispose();
                dataReader = null;
            }
            disposed = true;
        }
    }
}
