using System;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configure
{
    internal class TimeConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;
        private readonly Time _time;

        public TimeConsoleCommand(IConnection connection)
            : base("Time", "Set scanner to current time and date.")
        {
            _connection = connection;
            _time = new Time(connection);
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