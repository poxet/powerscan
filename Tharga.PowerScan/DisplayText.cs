using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tharga.PowerScan
{
    public class DisplayText
    {
        public enum FontSize
        {
            Large,
            Normal,
            Medium
        }

        private const int MaximumNumberOfLines = 5;

        private readonly Tuple<FontSize, string>[] _lines;

        public DisplayText()
            : this(new Tuple<FontSize, string>[] { })
        {
        }

        public DisplayText(IEnumerable<string> lines)
            : this(lines.Select(x => new Tuple<FontSize, string>(FontSize.Normal, x)).ToArray())
        {
        }

        public DisplayText(IEnumerable<Tuple<FontSize, string>> lines)
        {
            _lines = new Tuple<FontSize, string>[MaximumNumberOfLines];
            Clear();

            var i = 0;
            foreach (var line in lines)
            {
                if (i < MaximumNumberOfLines)
                {
                    _lines[i++] = line;
                }
            }
        }

        public int NumberOfLines => MaximumNumberOfLines;

        internal string TextCommandData
        {
            get
            {
                var sb = new StringBuilder();
                var row = 0;
                foreach (var line in _lines)
                {
                    row++;
                    sb.Append(Constants.Esc + $"[{row};1H"); //Place in position
                    sb.Append(GetFontCommand(line.Item1)); //Set font
                    sb.Append(Constants.Esc + "[2D");
                    if (line.Item2 != null)
                    {
                        var count = GetNumberOfCharsOnLine(line.Item1) - line.Item2.Length;
                        if (count < 0) count = 0;
                        sb.Append(line.Item2 + new string(' ', count));
                    }
                }

                var response = sb.ToString();
                return response;
            }
        }

        public int GetNumberOfCharsOnLine(FontSize fontSize)
        {
            switch (fontSize)
            {
                case FontSize.Large:
                    return 11;
                case FontSize.Normal:
                    return 22;
                case FontSize.Medium:
                    return 17;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetFontCommand(FontSize font)
        {
            switch (font)
            {
                case FontSize.Large:
                    return Constants.FontLarge;
                case FontSize.Normal:
                    return Constants.FontNormal;
                case FontSize.Medium:
                    return Constants.FontMedium;
                default:
                    throw new ArgumentOutOfRangeException($"The font {font} is not supported.");
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _lines.Length; i++)
            {
                _lines[i] = GetEmptyLine();
            }
        }

        public string GetText(int lineNumber)
        {
            return _lines[lineNumber].Item2;
        }

        public void SetText(int lineNumber, string data, FontSize fontSize, bool truncate = true)
        {
            if (lineNumber < 0 && !truncate) throw new InvalidOperationException("The line number needs to be at least 0.");
            if (lineNumber > NumberOfLines - 1 && !truncate) throw new InvalidOperationException("The line number is outside of the display.");
            if (data.Length > GetNumberOfCharsOnLine(fontSize))
            {
                if (truncate)
                {
                    data = data.Substring(0, GetNumberOfCharsOnLine(fontSize));
                }
                else
                {
                    throw new InvalidOperationException("The number of characteds are too great to display on the line.");
                }
            }

            if (fontSize == FontSize.Large)
            {
                if (lineNumber == MaximumNumberOfLines - 1)
                {
                    //Cannot write large text on the last line
                    if (!truncate)
                    {
                        throw new InvalidOperationException("Data with large font cannot be used on the last line.");
                    }

                    _lines[lineNumber] = GetEmptyLine();
                    return;
                }

                //If we are using the font size large, it takes up two lines. Therefore the line after should be clered.
                _lines[lineNumber + 1] = new Tuple<FontSize, string>(FontSize.Normal, null);
            }
            else
            {
                //If writing small text. Check if there was large text there before, if so, clear the line after that too.
                if (_lines[lineNumber].Item1 == FontSize.Large)
                {
                    _lines[lineNumber + 1] = GetEmptyLine();
                }

                //If the line above was large text, then clear that line too
                if (lineNumber > 0)
                {
                    if (_lines[lineNumber - 1].Item1 == FontSize.Large)
                    {
                        _lines[lineNumber - 1] = GetEmptyLine();
                    }
                }
            }

            _lines[lineNumber] = new Tuple<FontSize, string>(fontSize, data); // + new string('-', GetNumberOfCharsOnLine(fontSize) - data.Length + 1));
        }

        private Tuple<FontSize, string> GetEmptyLine()
        {
            //return new Tuple<FontSize, string>(FontSize.Normal, new string('.', GetNumberOfCharsOnLine(FontSize.Normal)));
            return new Tuple<FontSize, string>(FontSize.Normal, string.Empty);
        }
    }
}