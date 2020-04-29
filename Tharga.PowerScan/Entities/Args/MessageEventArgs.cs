using System;

namespace Tharga.PowerScan.Entities.Args
{
    public class MessageEventArgs : EventArgs
    {
        public Exception Exception { get; }
        public string Message { get; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public MessageEventArgs(Exception exception)
        {
            Exception = exception;
            Message = exception.Message;
        }
    }
}