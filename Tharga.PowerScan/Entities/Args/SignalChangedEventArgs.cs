using System;

namespace Tharga.PowerScan.Entities.Args
{
    public class SignalChangedEventArgs : EventArgs
    {
        public enum SignalState
        {
            Online,
            Offline
        }

        public SignalChangedEventArgs(SignalState signalState)
        {
            State = signalState;
        }

        public SignalState State { get; }
    }
}