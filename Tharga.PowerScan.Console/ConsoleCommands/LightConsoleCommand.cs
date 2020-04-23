using System.Collections.Generic;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands
{
    internal class LightConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;
        private readonly Light _light;

        public LightConsoleCommand(IConnection connection)
            : base("Light", "Blink the light on the unit")
        {
            _connection = connection;
            _light = new Light(connection);
        }

        public override bool CanExecute(out string reasonMessage)
        {
            var response = _connection.IsOpen;
            reasonMessage = response ? "open" : "not open";
            return response;
        }

        public override void Invoke(string[] param)
        {
            var color = QueryParam("Color", param, new Dictionary<LightColor, string>
            {
                {LightColor.Green, LightColor.Green.ToString()},
                {LightColor.Red, LightColor.Red.ToString()}
            });

            _light.Blink(color);
        }
    }
}