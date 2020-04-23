using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Connection
{
    internal class CloseConnectionConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public CloseConnectionConsoleCommand(IConnection connection)
            : base("Close", "Close the connection.")
        {
            _connection = connection;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            if (!_connection.IsOpen)
            {
                reasonMessage = "Not connected.";
                return false;
            }

            return base.CanExecute(out reasonMessage);
        }

        public override void Invoke(string[] param)
        {
            _connection.Close();
        }
    }
}