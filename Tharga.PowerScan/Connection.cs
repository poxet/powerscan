using System;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;
using Tharga.PowerScan.Interfaces;

namespace Tharga.PowerScan
{
    public class Connection : IConnection
    {
        public Connection()
        {
        }

        public Connection(Configuration configuration)
            : this(new SerialPortAgent(configuration))
        {
        }

        internal Connection(ISerialPortAgent serialPortAgent)
        {
            SerialPortAgent = serialPortAgent;
        }

        public bool IsOpen => SerialPortAgent != null && SerialPortAgent.IsOpen;
        public string OpenPortName => SerialPortAgent != null ? SerialPortAgent.PortName : string.Empty;
        internal ISerialPortAgent SerialPortAgent { get; private set; }

        public event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;
        public event EventHandler<ScanEventArgs> ScanEvent;
        public event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        public event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        public event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        public event EventHandler<SignalChangedEventArgs> SignalChangedEvent;

        public void Open(Configuration configuration)
        {
            if (IsOpen) throw new InvalidOperationException("The port is already open.");
            if (SerialPortAgent != null)
            {
                SerialPortAgent.Dispose();
                SerialPortAgent = null;
            }

            SerialPortAgent = new SerialPortAgent(configuration);
            Open();
        }

        public void Open()
        {
            if (IsOpen) throw new InvalidOperationException("The port is already open.");
            SerialPortAgent.Open();
            SerialPortAgent.ScanEvent += ScanEvent;
            SerialPortAgent.ScanConfirmationNotreceivedEvent += ScanConfirmationNotreceivedEvent;
            SerialPortAgent.ButtonPressedEvent += ButtonPressedEvent;
            SerialPortAgent.ButtonConfirmationNotreceivedEvent += ButtonConfirmationNotreceivedEvent;
            SerialPortAgent.SignalChangedEvent += SignalChangedEvent;
            OnConnectionChangedEvent(new ConnectionChangedEventArgs(ConnectionChangedEventArgs.ConnectionStatus.Open, SerialPortAgent.PortName));
        }

        public void Close()
        {
            if (!IsOpen) throw new InvalidOperationException("The port is not open.");
            SerialPortAgent.Close();
            OnConnectionChangedEvent(new ConnectionChangedEventArgs(ConnectionChangedEventArgs.ConnectionStatus.Closed, "N/A"));
        }

        public void Dispose()
        {
            if (SerialPortAgent == null)
            {
                return;
            }

            if (SerialPortAgent.IsOpen)
            {
                Close();
            }

            SerialPortAgent.Dispose();
        }

        protected virtual void OnConnectionChangedEvent(ConnectionChangedEventArgs e)
        {
            ConnectionChangedEvent?.Invoke(this, e);
        }
    }
}