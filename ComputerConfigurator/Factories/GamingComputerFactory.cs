using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerConfigurator.Factories
{
    public class GamingComputerFactory : IComputerFactory
    {
        public Computer CreateComputer()
        {
            return new ComputerBuilder()
                .WithCPU("AMD Ryzen 7 7800X3D")
                .WithRAM(32)
                .WithGPU("NVIDIA GeForce RTX 4080")
                .WithComponent("Игровая клавиатура RGB")
                .WithComponent("Игровая мышь")
                .WithComponent("Игровая Гарнитура")
                .WithComponent("Ультра крутой Коврик для мыши")
                .Build();
        }
    }
}
