using System;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;

namespace Tharga.PowerScan.Interfaces
{
    public interface IConnection : IDisposable
    {
        string OpenPortName { get; }
        bool IsOpen { get; }
        IConfiguration Configuration { get; }

        void Close();
        void Open(Transport configuration);
        void Command(string command);

        event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;
        event EventHandler<ScanEventArgs> ScanEvent;
        event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        event EventHandler<SignalChangedEventArgs> SignalChangedEvent;
        event EventHandler<MessageEventArgs> MessageEvent;
        event EventHandler<ConfigurationEventArgs> ConfigurationEvent;
    }
}