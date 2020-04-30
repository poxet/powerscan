using Tharga.PowerScan.Interfaces;

namespace Tharga.PowerScan
{
    public class Configuration : IConfiguration
    {
        private readonly IConnection _connection;

        public Configuration(IConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            End();
        }

        public void Start()
        {
            _connection.Command("$S");
        }

        public void End()
        {
            _connection.Command("$Ar");
        }
    }
}