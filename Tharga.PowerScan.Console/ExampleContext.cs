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
            var configuration = new Configuration();
            Connection = new Connection(configuration);

            Connection.ScanEvent += ScanEvent;
            Connection.ButtonPressedEvent += ButtonPressedEvent;
            //powerScanConnection.SignalChangedEvent += SignalChangedEvent;
            Connection.ConnectionChangedEvent += ConnectionChangedEvent;
            //powerScanConnection.ButtonConfirmationNotreceivedEvent += PowerScanConnection_ButtonConfirmationNotreceivedEvent;
            //powerScanConnection.ScanConfirmationNotreceivedEvent += PowerScanConnection_ScanConfirmationNotreceivedEvent;
            if (!string.IsNullOrEmpty(Connection.OpenPortName))
            {
                Connection.Open(configuration);
            }
            else
            {
                _console.OutputWarning("Not connected to any serial port. Use the connection command.");
            }
        }

        private void ScanEvent(object sender, ScanEventArgs e)
        {
            try
            {
                _console.OutputEvent("Data received from scanner: " + e.Data);
                var displayText = new DisplayText();
                displayText.SetText(2, "S: " + e.Data, DisplayText.FontSize.Normal);
                e.Confirm(displayText, true);
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }

        private void ButtonPressedEvent(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                _console.OutputEvent("Button pressed: " + e.Button);
                var displayText = new DisplayText();
                displayText.SetText(3, "B: " + e.Button, DisplayText.FontSize.Normal);
                e.Confirm(null, true);
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }

        public IConnection Connection { get; }

        private void ConnectionChangedEvent(object sender, ConnectionChangedEventArgs e)
        {
            try
            {
                _console.OutputEvent("Connection changed. " + e.Connection + ", port: " + e.PortName);
            }
            catch (Exception exception)
            {
                _console.OutputError(exception);
            }
        }
    }
}