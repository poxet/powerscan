using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Configuration
{
    internal class ConfigurationConsoleCommands : ContainerCommandBase
    {
        public ConfigurationConsoleCommands()
            : base("Configuration")
        {
            RegisterCommand<ConfigurationStartConsoleCommand>();
            RegisterCommand<ConfigurationEndConsoleCommand>();
            RegisterCommand<ConfigurationGetConsoleCommand>();
            RegisterCommand<ConfigurationSetConsoleCommand>();
            RegisterCommand<ConfigurationMuteConsoleCommand>();
            RegisterCommand<ConfigurationLoudConsoleCommand>();
        }
    }
}