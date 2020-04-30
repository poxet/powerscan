using System.Collections.Generic;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationGetConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public ConfigurationGetConsoleCommand(IConnection connection)
            : base("Get")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            var command = QueryParam<string>("Command", param, new Dictionary<string, string>
            {
                {"$cKBCO", "Country"},
                {"$cBPVO", "Good read beep volume"}
            });
            _connection.Command(command);
        }
    }
}