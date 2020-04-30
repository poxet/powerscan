using System;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Time
{
    internal class TimeSetConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;
        private readonly PowerScan.Time _time;

        public TimeSetConsoleCommand(IConnection connection)
            : base("Set", "Set scanner to current time and date.")
        {
            _connection = connection;
            _time = new PowerScan.Time(connection);
        }

        public override bool CanExecute(out string reasonMessage)
        {
            var response = _connection.IsOpen;
            reasonMessage = response ? "open" : "not open";
            return response;
        }

        public override void Invoke(string[] param)
        {
            var time = DateTime.Now;
            _time.SetTimeAndDate(time);
        }
    }
}