using FigureFactory.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FigureFactory.Factories
{
    public class GreenFactory : IFigureFactory
    {
        public Circle CreateCircle()
        {
            return new Circle { Color = Colors.Green };
        }

        public Square CreateSquare()
        {
            return new Square { Color = Colors.Green };
        }

        public Triangle CreateTriangle()
        {
            return new Triangle { Color = Colors.Green };
        }
    }
}
