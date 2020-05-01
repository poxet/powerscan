using System.Collections.Generic;
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
            var row = QueryParam("Row", param, new Dictionary<int, string>
            {
                {0, "0"},
                {1, "1"},
                {2, "2"},
                {3, "3"},
                {4, "4"}
            });

            var text = QueryParam<string>("Text", param);
            var font = QueryParam<DisplayText.FontSize>("Font", param);

            var dt = new DisplayText();
            dt.SetText(row, text, font);
            _text.WriteText(dt);
        }
    }
}