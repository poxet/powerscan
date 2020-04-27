using System.Collections.Generic;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configure
{
    internal class TimeConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;
        private readonly Time _time;

        public TimeConsoleCommand(IConnection connection)
            : base("Time", "Set time in the unit")
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
            var index = 0;
            var time = QueryParam("Time", GetParam(param, index++), new Dictionary<TimeSet, string>
            {
                {TimeSet.Date, TimeSet.Date.ToString()},
                {TimeSet.Time, TimeSet.Time.ToString()},
                {TimeSet.CurrentTime, TimeSet.CurrentTime.ToString()}
            });

            _time.ChangeTime(time);
        }
    }
}