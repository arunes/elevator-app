using System;

namespace Elevator.Models
{
    public class ElevatorException : Exception
    {
        public string ErrorCode { get; set; } = "GEN";

        public ElevatorException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ElevatorException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}