using Tharga.PowerScan.Interfaces;

namespace Tharga.PowerScan
{
    public class Text
    {
        private readonly Connection _connection;

        public Text(IConnection connection)
        {
            _connection = connection as Connection;
        }

        public void WriteText(DisplayText displayText)
        {
            _connection.SerialPortAgent.Command(displayText.TextCommandData);
        }
    }
}