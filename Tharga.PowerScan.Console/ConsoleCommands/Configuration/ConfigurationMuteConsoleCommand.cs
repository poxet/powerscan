using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationMuteConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public ConfigurationMuteConsoleCommand(IConnection connection)
            : base("Mute")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            _connection.Command("$CBPVO00"); //Good Read Beep Volume
            _connection.Command("$CBTAB00"); //Ack Beep
            _connection.Command("$CBPFR00"); //Good Read Beep Frequenzy
            _connection.Command("$CBPPU00"); //Power On Alter
        }
    }
}