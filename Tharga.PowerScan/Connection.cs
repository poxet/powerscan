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

        public Connection(Transport configuration)
            : this(new SerialPortAgent(configuration))
        {
        }

        internal Connection(ISerialPortAgent serialPortAgent)
        {
            SerialPortAgent = serialPortAgent;
            Configuration = new Configuration(this);
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
        public event EventHandler<MessageEventArgs> MessageEvent;
        public event EventHandler<ConfigurationEventArgs> ConfigurationEvent;

        public void Command(string command)
        {
            command = command.Replace("[ESC]", Constants.Esc);
            SerialPortAgent.Command(command);
        }

        public IConfiguration Configuration { get; }

        public void Open(Transport configuration)
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
            SerialPortAgent.ScanEvent += ScanEvent;
            SerialPortAgent.ScanConfirmationNotreceivedEvent += ScanConfirmationNotreceivedEvent;
            SerialPortAgent.ButtonPressedEvent += ButtonPressedEvent;
            SerialPortAgent.ButtonConfirmationNotreceivedEvent += ButtonConfirmationNotreceivedEvent;
            SerialPortAgent.SignalChangedEvent += SignalChangedEvent;
            SerialPortAgent.ConnectionChangedEvent += ConnectionChangedEvent;
            SerialPortAgent.MessageEvent += MessageEvent;
            SerialPortAgent.ConfigurationEvent += ConfigurationEvent;
            SerialPortAgent.Open();
        }

        public void Close()
        {
            if (!IsOpen) throw new InvalidOperationException("The port is not open.");
            SerialPortAgent.Close();
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
    }
}