using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationStartConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public ConfigurationStartConsoleCommand(IConnection connection)
            : base("Start")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            _connection.Configuration.Start();
            OutputInformation("Connection mode started.");
        }
    }
}