namespace Tharga.PowerScan.Entities
{
    internal class ConfirmationResponse
    {
        public ConfirmationResponse(bool confirmationReceived, DisplayText confirmationMessage, bool isSuccess)
        {
            ConfirmationReceived = confirmationReceived;
            ConfirmationMessage = confirmationMessage;
            IsSuccess = isSuccess;
        }

        public bool ConfirmationReceived { get; }
        public DisplayText ConfirmationMessage { get; }
        public bool IsSuccess { get; }
    }
}