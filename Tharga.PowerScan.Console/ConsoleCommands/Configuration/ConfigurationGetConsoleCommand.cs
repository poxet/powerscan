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
                {"$cBPVO", "Good Read Beep Volume"},
                {"$cBPLE", "Good Read Beep Length" },
                {"$cLSSP", "Green Spot Duration" },
                {"$cLAGL", "Good Read Led Duration" },
                //{"$cKBCO", "Country"},
            });
            OutputInformation($"Sending command: {command}");
            _connection.Command(command);
        }
    }
}