using System.Collections.Generic;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationSetConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public ConfigurationSetConsoleCommand(IConnection connection)
            : base("Set")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            var command = QueryParam<string>("Command", param, new Dictionary<string, string>
            {
                {"$CKBCO", "Country"},
                {"$CBPVO", "Good read beep volume"},
            });
            var value = QueryParam<string>("Value", param);
            if (value.Length != 2)
            {
                OutputWarning("Value needs to be two characters");
                return;
            }
            //var volume = QueryParam("Volume", param, new Dictionary<string, string>
            //{
            //    {"00", "Off"},
            //    {"01", "Low"},
            //    {"02", "Medium"},
            //    {"03", "High"},
            //});
            _connection.Command($"{command}{value}");
        }
    }
}