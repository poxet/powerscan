using System;

namespace Tharga.PowerScan.Entities.Args
{
    public class ScanConfirmationNotreceivedEventArgs : EventArgs
    {
        public ScanConfirmationNotreceivedEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}