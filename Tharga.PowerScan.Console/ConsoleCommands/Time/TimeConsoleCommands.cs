using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Time
{
    internal class TimeConsoleCommands : ContainerCommandBase
    {
        public TimeConsoleCommands()
            : base("Time")
        {
            RegisterCommand<TimeSetConsoleCommand>();
        }
    }
}