using System;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan
{
    public class Time
    {
        private readonly Connection _connection;

        public Time(IConnection connection)
        {
            _connection = connection as Connection;
        }

        public void ChangeTime(TimeSet time)
        {
            var settime = "";
            switch (time)
            {
                case TimeSet.Date:
                    Console.WriteLine("Enter date in format dd/mm/yy and press enter");
                    settime = Console.ReadLine();
                    _connection.SerialPortAgent.Write($"\x12\x1b[0p{settime}\x0d");
                    break;
                case TimeSet.Time:
                    Console.WriteLine("Enter time in format HH/MM and press enter. Note: Seconds are automatically set to 00.");
                    settime = Console.ReadLine();
                    _connection.SerialPortAgent.Write($"\x12\x1b[1p{settime}\x0d");
                    break;
                case TimeSet.CurrentTime:
                    Console.WriteLine("Current time set.");
                    settime = DateTime.Now.ToShortTimeString().Replace(":", string.Empty);
                    Console.WriteLine(settime);
                    _connection.SerialPortAgent.Write($"\x12\x1b[1p{settime}\x0d");
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The time set variant {time} is not supported, please try again.");
            }
        }
    }
}