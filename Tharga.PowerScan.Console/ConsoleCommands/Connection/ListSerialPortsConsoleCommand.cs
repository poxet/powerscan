using System.IO.Ports;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Connection
{
    internal class ListSerialPortsConsoleCommand : ActionCommandBase
    {
        public ListSerialPortsConsoleCommand()
            : base("Ports", "Lists available serial ports.")
        {
        }

        public override void Invoke(string[] param)
        {
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                OutputInformation(port);
            }

            OutputInformation($"There are {ports.Length} possible ports to use.");
        }
    }
}