using System;

namespace Tharga.PowerScan.Entities.Args
{
    public class ConnectionChangedEventArgs : EventArgs
    {
        public enum ConnectionStatus
        {
            Closed,
            Open
        }

        public ConnectionChangedEventArgs(ConnectionStatus connectionStatus, string portName)
        {
            Connection = connectionStatus;
            PortName = portName;
        }

        public ConnectionStatus Connection { get; }
        public string PortName { get; }
    }
}