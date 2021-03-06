﻿using System.IO.Ports;
using System.Linq;

namespace Tharga.PowerScan.Entities
{
    public class Transport
    {
        public Transport(string portName = null)
        {
            if (!string.IsNullOrEmpty(portName))
            {
                PortName = portName;
            }
            else
            {
                var ports = SerialPort.GetPortNames();
                if (ports.Length == 1)
                {
                    PortName = ports.Single();
                }
                else if (ports.Length > 1)
                {
                    PortName = "Auto";
                }
            }
        }

        public string PortName { get; }
        public Parity Parity => Parity.None;
        public int BaudRate => 115200;
        public StopBits StopBits => StopBits.One;
        public int DataBits => 8;
    }
}