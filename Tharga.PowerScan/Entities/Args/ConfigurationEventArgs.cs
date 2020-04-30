using System;

namespace Tharga.PowerScan.Entities.Args
{
    public class ConfigurationEventArgs : EventArgs
    {
        public ConfigurationEventArgs(string command, string response)
        {
            Command = command;
            Response = response;
        }

        public string Command { get; }
        public string Response { get; }
    }
}