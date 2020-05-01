using System;
using Tharga.PowerScan.Interfaces;
using Tharga.PowerScan.Menu;
using Tharga.Toolkit.Console.Commands.Base;

namespace Tharga.PowerScan.Console.ConsoleCommands.Simulator
{
    internal class SimulatorStartConsoleCommand : ActionCommandBase
    {
        private readonly IConnection _connection;

        public SimulatorStartConsoleCommand(IConnection connection)
            : base("Start")
        {
            _connection = connection;
        }

        public override void Invoke(string[] param)
        {
            //NOTE: Build menu tree for the simulator.
            var line1 = string.Empty;
            var menu = new MainMenu((sender, data) =>
            {
                line1 = data;
                switch (data)
                {
                    case Constants.Up:
                    case Constants.Down:
                    case Constants.Select:
                    case Constants.Back:
                        break;
                    default:
                        if (data.ToLower().StartsWith("a"))
                            sender.Menu.Select("SubA");
                        else if (data.ToLower().StartsWith("b"))
                            sender.Menu.Select("SubB");
                        break;
                }
            });

            menu.AddNode(new MenuNode("A").AddNode(new MenuNode("A1").AddNode(new MenuNode("A11"))));
            menu.AddNode(new MenuNode("B", (sender, data) => { }).AddNode(new MenuNode("B1")));
            menu.AddNode(new MenuNode("C", (sender, data) => { }).AddConfirm("Yes", "No", (sender, data) => { }, true));

            var line2 = string.Empty;
            var subA = new SubMenu("SubA", (sender, data) =>
            {
                line2 = data;

                switch (data)
                {
                    case Constants.Up:
                    case Constants.Down:
                    case Constants.Select:
                        break;
                    case Constants.Back:
                        line2 = string.Empty;
                        sender.Menu.Select(string.Empty);
                        break;
                    default:
                        //TODO: Do some stuff
                        break;
                }
            });
            subA.AddNode(new MenuNode("Info").AddConfirm("Ok", "Cancel", (sender, data) => { }));
            menu.AddSubMenu(subA).AddSubMenu(new SubMenu("B"));

            var subB = new SubMenu("SubB", (s, e) => { });
            subB.AddConfirm("Ja", "Nej", (s, d) => { s.Menu.Select(null); });
            menu.AddSubMenu(subB);

            var buffer = string.Empty;
            var running = true;
            while (running)
            {
                var dt = new DisplayText(new[]
                {
                    "Simulator",
                    line1,
                    line2,
                    menu.Name,
                    menu.Path
                });

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

                var key = QueryKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        menu.Back();
                        break;
                    case ConsoleKey.RightArrow:
                        menu.Select();
                        break;
                    case ConsoleKey.UpArrow:
                        menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.Down();
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                    case ConsoleKey.Enter:
                        menu.Handle(buffer);
                        buffer = string.Empty;
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