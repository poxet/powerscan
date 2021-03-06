﻿using System;
using System.IO.Ports;
using System.Threading.Tasks;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;
using Timer = System.Timers.Timer;

namespace Tharga.PowerScan
{
    internal class SerialPortAgent : ISerialPortAgent
    {
        private readonly Transport _transport;
        private SerialPort _serialPort;
        private int _responseTimeoutInMilliseconds;
        private bool _hasSignal;

        public SerialPortAgent(Transport transport)
        {
            _transport = transport;
            MonitorConnectionState();
        }

        public bool IsOpen => _serialPort != null && _serialPort.IsOpen;
        private bool _lastOpenState;
        private string _buffer;
        private string _lastCommand;
        public string PortName => _transport.PortName;

        public event EventHandler<ButtonPressedEventArgs> ButtonPressedEvent;
        public event EventHandler<ButtonConfirmationNotreceivedEventArgs> ButtonConfirmationNotreceivedEvent;
        public event EventHandler<ScanEventArgs> ScanEvent;
        public event EventHandler<ScanConfirmationNotreceivedEventArgs> ScanConfirmationNotreceivedEvent;
        public event EventHandler<SignalChangedEventArgs> SignalChangedEvent;
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChangedEvent;
        public event EventHandler<MessageEventArgs> MessageEvent;
        public event EventHandler<ConfigurationEventArgs> ConfigurationEvent;

        public void Open()
        {
            var port = _transport.PortName;
            if (port == "Auto")
            {
                var ports = SerialPort.GetPortNames();
                foreach (var p in ports)
                {
                    _serialPort = Start(p);
                    if (_serialPort != null)
                        break;
                }

                if (_serialPort == null)
                {
                    MessageEvent?.Invoke(this, new MessageEventArgs("Could not automatically connect."));
                    return;
                }
            }
            else
            {
                _serialPort = Start(port);
            }

            _hasSignal = true;
            SendConnectionChanged();

            //TODO: Query the settings of the scanner.
            //When a barcode is read, the scanner waits for a while before it gives up waiting for a response.
            //This is timeout is what should be used here. Ask the scanner for this setting and assign this variable.
            //Decrease the value a little bit, so that there is time for the code to execute. Perhaps 500ms or so.
            _responseTimeoutInMilliseconds = 3000;
        }

        private SerialPort Start(string port)
        {
            SerialPort serialPort = null;
            try
            {
                serialPort = new SerialPort(port)
                {
                    Parity = _transport.Parity,
                    BaudRate = _transport.BaudRate,
                    StopBits = _transport.StopBits,
                    DataBits = _transport.DataBits
                };
                serialPort.Open();

                //TODO: Wait for regular data response (Like when a button is pressed or a scan performed)
                serialPort.Write($"$+$!{Constants.End}");
                System.Threading.Thread.Sleep(100);
                var result = serialPort.ReadExisting();

                if (result.StartsWith("BC"))
                {
                    MessageEvent?.Invoke(this, new MessageEventArgs(result));
                    serialPort.PinChanged += _serialPort_PinChanged;
                    serialPort.ErrorReceived += _serialPort_ErrorReceived;
                    serialPort.DataReceived += SerialPortDataReceived;
                    serialPort.Disposed += _serialPort_Disposed;
                    return serialPort;
                }

                serialPort.Close();
                serialPort.Dispose();
                return null;
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke(this, new MessageEventArgs(e.Message));
                serialPort?.Dispose();
                return null;
            }
        }

        private void MonitorConnectionState()
        {
            var timer = new Timer {Interval = 3000};
            timer.Elapsed += (s, e) => { SendConnectionChanged(); };
            timer.Start();
        }

        private void SendConnectionChanged()
        {
            if (_lastOpenState == IsOpen) return;
            _lastOpenState = IsOpen;
            ConnectionChangedEvent?.Invoke(this, new ConnectionChangedEventArgs(IsOpen ? ConnectionChangedEventArgs.ConnectionStatus.Open : ConnectionChangedEventArgs.ConnectionStatus.Closed, _transport.PortName));
        }

        public void Close()
        {
            _serialPort.Close();
            _hasSignal = false;
            SendConnectionChanged();
        }

        public void Write(string data)
        {
            _serialPort.Write(data);
        }

        public void Command(string data)
        {
            _lastCommand = data;
            Write($"{data}{Constants.End}");
        }

        public void Dispose()
        {
            _serialPort?.Dispose();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _buffer += _serialPort.ReadExisting();

            if (_buffer.EndsWith("\r"))
            {
                var data = _buffer.TrimEnd('\r');

                if (data.StartsWith("$"))
                {
                    ConfigurationEvent?.Invoke(this, new ConfigurationEventArgs(_lastCommand, data));
                    _buffer = null;
                    return;
                }
                _buffer = null;

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
                _serialPort.Write($"OK{Constants.End}");
                if (response.ConfirmationMessage != null)
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
            try
            {
                var scanEventArgs = new ScanEventArgs(data);
                Task.Run(() => { ScanEvent?.Invoke(sender, scanEventArgs); });

                var response = scanEventArgs.Wait(_responseTimeoutInMilliseconds);
                if (response.ConfirmationReceived)
                {
                    _serialPort.Write($"OK{Constants.End}");
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
            catch (Exception e)
            {
                MessageEvent?.Invoke(this, new MessageEventArgs(e));
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