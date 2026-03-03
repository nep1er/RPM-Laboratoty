using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerConfigurator.Factories
{
    public class OfficeComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("Intel Core i3-12100")
                .WithRAM(8)
                .WithGPU("Интегрированная Intel UHD Graphics 730")
                .WithComponent("Клавиатура")
                .WithComponent("Мышь")
                .WithComponent("Офисный пакет ПО")
                .Build();
        }
    }
}
