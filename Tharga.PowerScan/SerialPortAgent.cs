using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Timers;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan
{
    internal class SerialPortAgent : ISerialPortAgent
    {
        private readonly Configuration _configuration;
        private SerialPort _serialPort;
        private int _responseTimeoutInMilliseconds;
        private bool _hasSignal;

        public SerialPortAgent(Configuration configuration)
        {
            _configuration = configuration;
            MonitorConnectionState();
        }

        public bool IsOpen => _serialPort != null && _serialPort.IsOpen;
        private bool _lastOpenState;
        public string PortName => _configuration.PortName;

        public event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        public event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        public event EventHandler<ScanEventArgs> ScanEvent;
        public event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        public event EventHandler<SignalChangedEventArgs> SignalChangedEvent;
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;

        public void Open()
        {
            _serialPort = new SerialPort(_configuration.PortName)
            {
                Parity = _configuration.Parity,
                BaudRate = _configuration.BaudRate,
                StopBits = _configuration.StopBits,
                DataBits = _configuration.DataBits
            };

            _serialPort.PinChanged += _serialPort_PinChanged;
            _serialPort.ErrorReceived += _serialPort_ErrorReceived;
            _serialPort.DataReceived += SerialPortDataReceived;
            _serialPort.Disposed += _serialPort_Disposed;
            _serialPort.Open();
            _hasSignal = true;

            //TODO: Query the settings of the scanner.
            //When a barcode is read, the scanner waits for a while before it gives up waiting for a response.
            //This is timeout is what should be used here. Ask the scanner for this setting and assign this variable.
            //Decrease the value a little bit, so that there is time for the code to execute. Perhaps 500ms or so.
            _responseTimeoutInMilliseconds = 3000;
        }

        private void MonitorConnectionState()
        {
            var timer = new Timer {Interval = 3000};
            timer.Elapsed += (s, e) =>
            {
                if (_lastOpenState == IsOpen) return;
                _lastOpenState = IsOpen;
                ConnectionChangedEvent?.Invoke(this, new ConnectionChangedEventArgs(IsOpen ? ConnectionChangedEventArgs.ConnectionStatus.Open : ConnectionChangedEventArgs.ConnectionStatus.Closed, _configuration.PortName));
            };
            timer.Start();
        }

        public void Close()
        {
            _serialPort.Close();
            _hasSignal = false;
        }

        public void Write(string data)
        {
            _serialPort.Write(data);
        }

        public void Command(string data)
        {
            Write(data + Constants.End);
        }

        public void Dispose()
        {
            _serialPort?.Dispose();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = _serialPort.ReadExisting();
            data = data.Substring(0, data.Length - 1);

            switch (data)
            {
                case "S1":
                case "F1":
                    ButtonPressedAction(sender, Button.F1);
                    break;
                case "S2":
                case "F2":
                    ButtonPressedAction(sender, Button.F2);
                    break;
                case "Up":
                    ButtonPressedAction(sender, Button.Up);
                    break;
                case "Dwn":
                case "Down":
                    ButtonPressedAction(sender, Button.Down);
                    break;
                default:
                    ScanAction(sender, data);
                    break;
            }
        }

        private void _serialPort_Disposed(object sender, EventArgs e)
        {
            //Debug.WriteLine("_serialPort_Disposed");
        }

        private void ButtonPressedAction(object sender, Button button)
        {
            var buttonPressedEventArgs = new ButtonPressedEventArgs(button);
            Task.Run(() => { ButtonPressedEvent?.Invoke(sender, buttonPressedEventArgs); });

            var response = buttonPressedEventArgs.Wait(_responseTimeoutInMilliseconds);
            if (response.ConfirmationReceived)
            {
                _serialPort.Write("OK\x0d");
                _serialPort.Write(response.ConfirmationMessage.TextCommandData + Constants.End);

                if (!response.IsSuccess)
                {
                    _serialPort.Write(Constants.BeepLongLowTone + Constants.LedRedOn + Constants.End);
                }
            }
            else
            {
                ButtonConfirmationNotreceivedEvent?.Invoke(this, new ButtonConfirmationNotreceivedEventArgs(button));
            }
        }

        private void ScanAction(object sender, string data)
        {
            var scanEventArgs = new ScanEventArgs(data);
            Task.Run(() => { ScanEvent?.Invoke(sender, scanEventArgs); });

            var response = scanEventArgs.Wait(_responseTimeoutInMilliseconds);
            if (response.ConfirmationReceived)
            {
                _serialPort.Write("OK\x0d");
                _serialPort.Write(response.ConfirmationMessage.TextCommandData + Constants.End);

                if (!response.IsSuccess)
                {
                    _serialPort.Write(Constants.BeepBadReadTone + Constants.LedRedOn + Constants.End);
                }
            }
            else
            {
                ScanConfirmationNotreceivedEvent?.Invoke(this, new ScanConfirmationNotreceivedEventArgs(data));
            }
        }

        private void _serialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            var signalState = false;

            switch (e.EventType)
            {
                case SerialPinChange.Break:
                    break;
                case SerialPinChange.CDChanged:
                    signalState = _serialPort.CtsHolding;
                    break;
                case SerialPinChange.CtsChanged:
                    signalState = _serialPort.CDHolding;
                    break;
                case SerialPinChange.DsrChanged:
                    signalState = _serialPort.DsrHolding;
                    break;
                case SerialPinChange.Ring:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_hasSignal != signalState)
            {
                SignalChangedEvent?.Invoke(this, new SignalChangedEventArgs(signalState ? SignalChangedEventArgs.SignalState.Online : SignalChangedEventArgs.SignalState.Offline));
                _hasSignal = signalState;
            }
        }

        private void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //Debug.WriteLine("_serialPort_ErrorReceived");
        }
    }
}