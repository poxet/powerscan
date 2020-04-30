using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands
{
    internal class TextConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;
        private readonly Text _text;

        public TextConsoleCommand(IConnection connection)
            : base("Text", "Display some text on the unit")
        {
            _connection = connection;
            _text = new Text(_connection);
        }

        public override bool CanExecute(out string reasonMessage)
        {
            var response = _connection.IsOpen;
            reasonMessage = response ? "open" : "not open";
            return response;
        }

        public override void Invoke(string[] param)
        {
            var input = QueryParam<string>("Text", param);
            var dt = new DisplayText(new[] { input, "B", null, "", "D" });
            _text.WriteText(dt);
        }
    }
}