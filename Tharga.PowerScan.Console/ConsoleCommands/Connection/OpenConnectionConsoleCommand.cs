using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Tharga.PowerScan.Entities;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Connection
{
    internal class OpenConnectionConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public OpenConnectionConsoleCommand(IConnection connection)
            : base("Open", "Opens a connection.")
        {
            _connection = connection;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            if (_connection.IsOpen)
            {
                reasonMessage = "Already connected.";
                return false;
            }

            return base.CanExecute(out reasonMessage);
        }

        public override void Invoke(string[] param)
        {
            var portName = QueryParam("Port", param, SerialPort.GetPortNames().Select(x => new KeyValuePair<string, string>(x, x)));
            _connection.Open(new Configuration(portName));
        }
    }
}