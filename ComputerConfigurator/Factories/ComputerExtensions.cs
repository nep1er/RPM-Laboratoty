using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerConfigurator.Factories
{
    public static class ComputerExtensions
    {
        public static Computer WithComponent(this Computer computer, string component)
        {
            computer.AdditionalComponents.Add(component);
            return computer;
        }
    }
}
