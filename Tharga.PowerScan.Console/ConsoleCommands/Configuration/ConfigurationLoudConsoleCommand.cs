using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationLoudConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public ConfigurationLoudConsoleCommand(IConnection connection)
            : base("Loud")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            _connection.Command("$CBPVO03"); //Good Read Beep Volume
        }
    }
}