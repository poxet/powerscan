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
            var input = QueryParam<string>("Txt", param);

            var dt = new DisplayText(new[] { input, "B", null, "", "D" });

            //var dt = new DisplayText();
            //
            //dt.SetText(0, input, DisplayText.FontSize.Normal);
            ////dt.SetText(1, "Medium", DisplayText.FontSize.Normal);
            ////dt.SetText(2, "Medium", DisplayText.FontSize.Medium);
            //dt.SetText(3, "ABCDEFGHIJKLMNOPQRSTUVXYZÅÄÖ", DisplayText.FontSize.Normal);
            //dt.SetText(4, "ABCDEFGHIJKLMNOPQRSTUVXYZÅÄÖ", DisplayText.FontSize.Medium);
            //
            ////var index = 0;
            ////for (var lineNumber = 0; lineNumber < dt.NumberOfLines; lineNumber++)
            ////{
            //////    var s = QueryParam<string>("Line " + lineNumber, GetParam(paramList, index++));
            //////    if (string.IsNullOrEmpty(s))
            //////        break;
            //////    var font = QueryParam("Font", GetParam(paramList, index++), new Dictionary<DisplayText.FontSize, string>
            //////    {
            //////        {DisplayText.FontSize.Large, DisplayText.FontSize.Large.ToString()},
            //////        {DisplayText.FontSize.Normal, DisplayText.FontSize.Normal.ToString()},
            //////        {DisplayText.FontSize.Medium, DisplayText.FontSize.Medium.ToString()}
            //////    });
            ////    var s = lineNumber.ToString();
            ////    var font = DisplayText.FontSize.Large;
            ////    dt.SetText(lineNumber, s, font);
            ////    if (font == DisplayText.FontSize.Large) lineNumber++;
            ////}

            _text.WriteText(dt);
        }
    }
}