using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerConfigurator.Factories
{
    public interface IComputerFactory
    {
        Computer CreateComputer();
    }
}
