using System.Runtime.Serialization;

namespace Common.Results
{
    [DataContract]
    public class ValidationResult
    {
        private bool success;
        private string message;

        [DataMember]
        public bool Success { get => success; set => success = value; }

        [DataMember]
        public string Message { get => message; set => message = value; }

        public ValidationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
