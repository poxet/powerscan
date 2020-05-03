namespace Tharga.PowerScan.Menu
{
    public class HandlerResult
    {
        public HandlerResult(DisplayText displayText, bool success)
        {
            DisplayText = displayText;
            Success = success;
        }

        public DisplayText DisplayText { get; }
        public bool Success { get; }
    }
}