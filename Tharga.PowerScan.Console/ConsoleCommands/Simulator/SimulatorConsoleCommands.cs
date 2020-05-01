using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Simulator
{
    internal class SimulatorConsoleCommands : ContainerCommandBase
    {
        public SimulatorConsoleCommands()
            : base("Simulator")
        {
            RegisterCommand<SimulatorStartConsoleCommand>();
        }
    }
}