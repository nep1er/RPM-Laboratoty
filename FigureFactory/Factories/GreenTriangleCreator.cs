using FigureFactory.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FigureFactory.Factories
{

    public class GreenTriangleCreator : TriangleCreator
    {
        public override Triangle CreateTriangle()
        {
            return new Triangle { Color = Colors.Green };
        }
    }
}
