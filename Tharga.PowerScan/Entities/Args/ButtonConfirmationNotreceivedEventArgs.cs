using System;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan.Entities.Args
{
    public class ButtonConfirmationNotreceivedEventArgs : EventArgs
    {
        public ButtonConfirmationNotreceivedEventArgs(Button button)
        {
            Button = button;
        }

        public Button Button { get; }
    }
}