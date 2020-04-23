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

            //powerScanConnection.ScanEvent += ScanEvent;
            //powerScanConnection.ButtonPressedEvent += ButtonPressedEvent;
            //powerScanConnection.SignalChangedEvent += SignalChangedEvent;
            Connection.ConnectionChangedEvent += ConnectionChangedEvent;
            //powerScanConnection.ButtonConfirmationNotreceivedEvent += PowerScanConnection_ButtonConfirmationNotreceivedEvent;
            //powerScanConnection.ScanConfirmationNotreceivedEvent += PowerScanConnection_ScanConfirmationNotreceivedEvent;
            if (!string.IsNullOrEmpty(Connection.OpenPortName))
            {
                Connection.Open(new Configuration());
            }
            else
            {
                _console.OutputWarning("Not connected to any serial port. Use the connection command.");
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