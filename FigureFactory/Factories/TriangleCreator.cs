using FigureFactory.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigureFactory.Factories
{
    public abstract class TriangleCreator
    {
        public abstract Triangle CreateTriangle();
    }
}
