using System;
using Tharga.PowerScan.Interfaces;

namespace Tharga.PowerScan
{
    public class Time
    {
        private readonly Connection _connection;

        public Time(IConnection connection)
        {
            _connection = connection as Connection;
        }

        public void SetTimeAndDate(DateTime time)
        {
            //Date
            var dt = time.ToString("dd-MM-yy").Replace("-", "/");
            _connection.SerialPortAgent.Write($"\x12\x1b[0p{dt}\x0d");

            //Time
            var tm = time.ToShortTimeString().Replace(":", string.Empty);
            _connection.SerialPortAgent.Write($"\x12\x1b[1p{tm}\x0d");
        }
    }
}