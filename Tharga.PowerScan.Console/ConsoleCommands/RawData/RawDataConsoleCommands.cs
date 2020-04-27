using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.RawData
{
    internal class RawDataConsoleCommands : ContainerCommandBase
    {
        public RawDataConsoleCommands()
            : base("Raw")
        {
            RegisterCommand<RawDataSendConsoleCommand>();
        }
    }
}