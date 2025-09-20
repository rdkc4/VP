using Common.Results;
using Common.Services.Data;
using Server.Exceptions;
using System;
using System.Configuration;
using System.IO;

namespace Server.Services.Data
{
    internal class DataWriter : IDataWriter
    {
        private readonly string PROCESSED_DIR_PATH = ConfigurationManager.AppSettings["ProcessedDataDir"] ?? "./../../ProcessedData";
        private readonly string VALID_FILE_NAME = ConfigurationManager.AppSettings["ValidSampleFile"] ?? "measurements_session";
        private readonly string REJECT_FILE_NAME = ConfigurationManager.AppSettings["RejectSampleFile"] ?? "rejects.csv";

        private StreamWriter validWriter = null;
        private StreamWriter rejectWriter = null;

        private bool disposed = false;

        public OperationResult Init()
        {
            try
            {
                if (!Directory.Exists(PROCESSED_DIR_PATH))
                {
                    Directory.CreateDirectory(PROCESSED_DIR_PATH);
                }

                validWriter = new StreamWriter(Path.Combine(PROCESSED_DIR_PATH, VALID_FILE_NAME), append: false)
                {
                    AutoFlush = true
                };

                rejectWriter = new StreamWriter(Path.Combine(PROCESSED_DIR_PATH, REJECT_FILE_NAME), append: false)
                {
                    AutoFlush = true
                };

                return new OperationResult(success: true, message: "");
            }
            catch (Exception ex)
            {
                return new OperationResult(success: false, message: ex.Message);
            }
        }

        public void WriteValidData(string data)
        {
            try
            {
                if (validWriter == null)
                {
                    throw new InvalidOperationException("Valid writer is not initialized");
                }

                validWriter.WriteLine(data);
            }
            catch (Exception ex)
            {
                throw new DataWriterException($"Failed to write valid data: {ex.Message}");
            }
        }

        public void WriteRejectedData(string data)
        {
            try
            {
                if (rejectWriter == null)
                {
                    throw new InvalidOperationException("Reject writer is not initialized");
                }

                rejectWriter.WriteLine(data);
            }
            catch (Exception ex)
            {
                throw new DataWriterException($"Failed to write rejected data: {ex.Message}");
            }
        }

        ~DataWriter()
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
            Console.WriteLine("[Data Writer]: Disposing...");
            if (disposed) return;

            if (disposing)
            {
                if (validWriter != null)
                {
                    validWriter.Dispose();
                    validWriter = null;
                }

                if (rejectWriter != null)
                {
                    rejectWriter.Dispose();
                    rejectWriter = null;
                }
            }
            disposed = true;
        }
    }
}
