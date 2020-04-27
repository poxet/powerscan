using System;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan
{
    public class Beep
    {
        private readonly Connection _connection;

        public Beep(IConnection connection)
        {
            _connection = connection as Connection;
        }

        public void MakeSound(BeepSound sound)
        {
            switch (sound)
            {
                case BeepSound.ShortLowtone:
                    _connection.SerialPortAgent.Command(Constants.BeepShortLowTone);
                    break;
                case BeepSound.ShortHightone:
                    _connection.SerialPortAgent.Command(Constants.BeepShortHighTone);
                    break;
                case BeepSound.LongLowtone:
                    _connection.SerialPortAgent.Command(Constants.BeepLongLowTone);
                    break;
                case BeepSound.GoodReadtone:
                    _connection.SerialPortAgent.Command(Constants.BeepGoodReadTone);
                    break;
                case BeepSound.BadReadtone:
                    _connection.SerialPortAgent.Command(Constants.BeepBadReadTone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The sound {sound} is not supported.");
            }
        }
    }
}