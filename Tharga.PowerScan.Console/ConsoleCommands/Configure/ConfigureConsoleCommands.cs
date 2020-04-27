using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configure
{
    internal class ConfigureConsoleCommands : ContainerCommandBase
    {
        public ConfigureConsoleCommands()
            : base("Configure")
        {
            RegisterCommand<TimeConsoleCommand>();
        }
    }
}