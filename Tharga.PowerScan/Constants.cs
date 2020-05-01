namespace Tharga.PowerScan
{
    internal class Constants
    {
        public const string Esc = "\x12\x1b";
        public const string End = "\x0d";

        public const string BeepShortHighTone = Esc + "[0q"; //Short high tone with short delay
        public const string BeepShortLowTone = Esc + "[1q"; //Short low tone with short delay
        public const string BeepLongLowTone = Esc + "[2q"; //Long low tone with short delay
        public const string BeepGoodReadTone = Esc + "[3q"; //Good read tone
        public const string BeepBadReadTone = Esc + "[4q"; //Bad read tone
        public const string Wait100Ms = Esc + "[5q"; //Wait for 100ms
        public const string LedGreenOn = Esc + "[6q"; //Turn on green led
        public const string LedGreenOff = Esc + "[7q"; //Turn off green led
        public const string LedRedOn = Esc + "[8q"; //Turn on red led
        public const string LedRedOff = Esc + "[9q"; //Turn off red led
        public const string Date = Esc + "[0p"; //Set Date
        public const string Time = Esc + "[1p"; //Set Time
        public const string ClearEntireDisplay = Esc + "[2J"; //Clears entire display and moves cursor to top left
        public const string FontLarge = Esc + "#4"; //Sets font to Large
        public const string FontNormal = Esc + "#5"; //Sets font to Normal
        public const string FontMedium = Esc + "#7"; //Sets font to Medium
        //public const string CR = Esc + "[G";
        //public const string CRLF = Esc + "E";
        //public const string DownOneRow = Esc + "[1B";
    }
}