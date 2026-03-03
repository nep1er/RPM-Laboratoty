using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerConfigurator.Factories
{
    public class HomeComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("Intel Core i5-13400")
                .WithRAM(16)
                .WithGPU("Intel Arc A750")
                .WithComponent("Wi-Fi адаптер")
                .WithComponent("Bluetooth адаптер")
                .WithComponent("Кардридер")
                .Build();
        }
    }
}
