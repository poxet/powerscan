using System;
using Tharga.PowerScan.Entities.Args;

namespace Tharga.PowerScan.Interfaces
{
    internal interface ISerialPortAgent : IDisposable
    {
        bool IsOpen { get; }
        string PortName { get; }
        event EventHandler<ScanEventArgs> ScanEvent;
        event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        event EventHandler<SignalChangedEventArgs> SignalChangedEvent;
        event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;
        void Open();
        void Close();
        void Write(string data);
        string Command(string data);
    }
}