using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.RawData
{
    internal class RawDataSendConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public RawDataSendConsoleCommand(IConnection connection)
            : base("Send")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            var data = QueryParam<string>("Command", param);
            _connection.Command(data);
        }
    }
}