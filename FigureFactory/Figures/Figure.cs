using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace FigureFactory.Figures
{
    public abstract class Figure
    {
        public Color Color { get; set; }

        public abstract UIElement CreateUIElement(double size = 50);
    }
}
