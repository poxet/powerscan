using System;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;

namespace Tharga.PowerScan.Interfaces
{
    public interface IConnection : IDisposable
    {
        string OpenPortName { get; }
        bool IsOpen { get; }
        void Close();
        void Open(Configuration configuration);
        event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;
        event EventHandler<ScanEventArgs> ScanEvent;
        event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        event EventHandler<SignalChangedEventArgs> SignalChangedEvent;
        void Command(string command);
    }
}