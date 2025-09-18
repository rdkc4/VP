using Client.Exceptions;
using Common.Services.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
