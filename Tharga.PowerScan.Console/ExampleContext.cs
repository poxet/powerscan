using System;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Entities.Args;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Consoles;

namespace Tharga.PowerScan.Console
{
    public class ExampleContext
    {
        private readonly ClientConsole _console;

        public ExampleContext(ClientConsole console)
        {
            _console = console;
            var transport = new Transport();
            Connection = new Connection(transport);

            Connection.ScanEvent += OnScanEvent;
            Connection.ButtonPressedEvent += OnButtonPressedEvent;
            Connection.SignalChangedEvent += OnSignalChangedEvent;
            Connection.ConnectionChangedEvent += OnConnectionChangedEvent;
            Connection.ButtonConfirmationNotreceivedEvent += OnButtonConfirmationNotreceivedEvent;
            Connection.ScanConfirmationNotreceivedEvent += OnScanConfirmationNotreceivedEvent;
            Connection.MessageEvent += OnMessageEvent;
            Connection.ConfigurationEvent += OnConfigurationEvent;
            if (!string.IsNullOrEmpty(Connection.OpenPortName))
            {
                Connection.Open(transport);
            }
            else
            {
                _console.OutputWarning("Not connected to any serial port. Use the connection command.");
            }
        }

        private void OnConfigurationEvent(object sender, ConfigurationEventArgs e)
        {
            _console.OutputInformation($"{e.Command}: {e.Response}");
        }

        public IConnection Connection { get; }

        private void OnScanConfirmationNotreceivedEvent(object sender, ScanConfirmationNotreceivedEventArgs e)
        {
            _console.OutputEvent($"Scan: {e.Data}");
        }

        private void OnButtonConfirmationNotreceivedEvent(object sender, ButtonConfirmationNotreceivedEventArgs e)
        {
            _console.OutputEvent($"Button: {e.Button}");
        }

        private void OnSignalChangedEvent(object sender, SignalChangedEventArgs e)
        {
            _console.OutputEvent($"Signal changed to {e.State}.");
        }

        private void OnMessageEvent(object sender, MessageEventArgs e)
        {
            if (e.Exception != null)
            {
                _console.OutputError(e.Exception);
            }
            else
            {
                _console.OutputInformation(e.Message);
            }
        }

        private void OnScanEvent(object sender, ScanEventArgs e)
        {
            try
            {
                _console.OutputEvent("OnScanEvent: " + e.Data);
                var displayText = new DisplayText();
                displayText.SetText(2, "S: " + e.Data, DisplayText.FontSize.Normal);
                e.Confirm(displayText, true);
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }

        private void OnButtonPressedEvent(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                _console.OutputEvent("OnButtonPressedEvent: " + e.Button);
                var displayText = new DisplayText();
                displayText.SetText(3, "B: " + e.Button, DisplayText.FontSize.Normal);
                e.Confirm(displayText, true);
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }

        private void OnConnectionChangedEvent(object sender, ConnectionChangedEventArgs e)
        {
            try
            {
                _console.OutputEvent($"Connection changed. {e.Connection}, port: {e.PortName}");
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }
    }
}