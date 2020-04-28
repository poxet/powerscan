using System;
using Tharga.PowerScan.Console.ConsoleCommands;
using Tharga.PowerScan.Console.ConsoleCommands.Configure;
using Tharga.PowerScan.Console.ConsoleCommands.Connection;
using Tharga.PowerScan.Console.ConsoleCommands.RawData;
using Tharga.PowerScan.Console.ConsoleCommands.Simulator;
using Tharga.PowerScan.Console.Helpers;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Commands;
using Tharga.Toolkit.Console.Consoles;
using Tharga.Toolkit.Console.Helpers;
using Tharga.Toolkit.Console.Interfaces;

namespace Tharga.PowerScan.Console
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using (var console = new ClientConsole())
            {
                var connection = new ExampleContext(console).Connection;
                var container = InjectionHelper.GetIocContainer(connection);

                var rootCommand = new RootCommand(console, new CommandResolver(type => (ICommand) container.Resolve(type)));
                rootCommand.RegisterCommand<ConnectionConsoleCommands>();
                rootCommand.RegisterCommand<ConfigureConsoleCommands>();
                rootCommand.RegisterCommand<SimulatorConsoleCommands>();
                rootCommand.RegisterCommand<LightConsoleCommand>();
                rootCommand.RegisterCommand<TextConsoleCommand>();
                rootCommand.RegisterCommand<BeepConsoleCommand>();
                rootCommand.RegisterCommand<RawDataConsoleCommands>();

                var engine = new CommandEngine(rootCommand);
                engine.Start(args);
            }
        }
    }
}