using System;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan
{
    public class Light
    {
        private readonly Connection _connection;

        public Light(IConnection connection)
        {
            _connection = connection as Connection;
        }

        public void Blink(LightColor colour)
        {
            switch (colour)
            {
                case LightColor.Green:
                    _connection?.SerialPortAgent.Write("\x12\x1b[6q\x0d"); // Color Green
                    break;
                case LightColor.Red:
                    _connection?.SerialPortAgent.Write("\x12\x1b[8q\x0d"); // Color Red
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The color {colour} is not supported.");
            }
        }
    }
}