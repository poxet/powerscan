using System;

namespace Tharga.PowerScan.Interfaces
{
    public interface IConfiguration : IDisposable
    {
        void Start();
        void End();
    }
}