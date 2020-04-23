using System;
using System.Threading;
using Tharga.PowerScan.Types;

namespace Tharga.PowerScan.Entities.Args
{
    public class ButtonPressedEventArgs : EventArgs
    {
        private readonly ManualResetEvent _confirmEvent = new ManualResetEvent(false);
        private DisplayText _confirmationMessage;
        private bool _isSuccess;
        private WaitStatus _waitStatus;

        public ButtonPressedEventArgs(Button button)
        {
            Button = button;
        }

        public Button Button { get; }

        public void Confirm(DisplayText confirmationMessage, bool isSuccess)
        {
            if (_confirmationMessage != null) throw new InvalidOperationException("This event has already been confirmed.");
            if (_waitStatus == WaitStatus.DoneWaiting) throw new TimeoutException("The confirm message came too late. The scanner has already given up waiting.");

            _isSuccess = isSuccess;
            _confirmationMessage = confirmationMessage ?? throw new InvalidOperationException("Cannot use null as confirmation message. Use 'string.Empty' if nothing is to be displayed on the scanner.");
            _confirmEvent.Set();
        }

        internal ConfirmationResponse Wait(int millisecondsTimeout)
        {
            try
            {
                var response = true;
                if (_confirmationMessage == null)
                {
                    _waitStatus = WaitStatus.Waiting;
                    response = _confirmEvent.WaitOne(millisecondsTimeout);
                }

                return new ConfirmationResponse(response, _confirmationMessage, _isSuccess);
            }
            finally
            {
                _waitStatus = WaitStatus.DoneWaiting;
            }
        }
    }
}