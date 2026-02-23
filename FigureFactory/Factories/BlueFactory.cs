using FigureFactory.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FigureFactory.Factories
{
    public class BlueFactory : IFigureFactory
    {
        public Circle CreateCircle()
        {
            return new Circle { Color = Colors.Blue };
        }

        public Square CreateSquare()
        {
            return new Square { Color = Colors.Blue };
        }

        public Triangle CreateTriangle()
        {
            return new Triangle { Color = Colors.Blue };
        }
    }
}
