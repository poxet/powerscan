using System;
using Microsoft.SqlServer.Server;
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

    internal class SimulatorStartConsoleCommand : ActionCommandBase
    {
        public SimulatorStartConsoleCommand()
            : base("Start")
        {
        }

        public override void Invoke(string[] param)
        {
            var buffer = string.Empty;
            var running = true;
            while (running)
            {
                var dt = new DisplayText(new[] {"1", "2", "3", "4", "5"});

                System.Console.Clear();
                OutputInformation("-----------------------");
                OutputInformation($"|{dt.GetText(0).Padd(21)}|");
                OutputInformation($"|{dt.GetText(1).Padd(21)}|");
                OutputInformation($"|{dt.GetText(2).Padd(21)}|");
                OutputInformation($"|{dt.GetText(3).Padd(21)}|");
                OutputInformation($"|{dt.GetText(4).Padd(21)}|");
                OutputInformation("|    S1T       S2T    |");
                OutputInformation("-----------------------");
                OutputInformation($"Buffer: {buffer}");

                var key = System.Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                    default:
                        buffer += key.KeyChar;
                        break;
                }
            }

            OutputInformation("Exiting simulator");
        }
    }
}