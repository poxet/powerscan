using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Tharga.PowerScan.Interfaces;
using Tharga.Toolkit.Console.Interfaces;

namespace Tharga.PowerScan.Console.Helpers
{
    internal class InjectionHelper
    {
        public static WindsorContainer GetIocContainer(IConnection connection)
        {
            var container = new WindsorContainer();
            container.Register(Classes.FromAssemblyInThisApplication(Assembly.GetAssembly(typeof(Program)))
                .IncludeNonPublicTypes()
                .BasedOn<ICommand>()
                //.Configure(x => console.Output(new WriteEventArgs("Registered in IOC: " + x.Implementation.Name, OutputLevel.Default, ConsoleColor.DarkRed)))
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(Component.For<IConnection>().Instance(connection).LifestyleSingleton());
            return container;
        }
    }
}