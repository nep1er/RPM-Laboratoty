using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Domain.Shapes
{
    public class Ellipse : GraphicObject
    {
        private float _x, _y, _radiusX, _radiusY;

        public Ellipse(IRenderingEngine engine, float x, float y, float rx, float ry)
            : base(engine)
        {
            _x = x; _y = y; _radiusX = rx; _radiusY = ry;
        }

        public override void Draw()
        {
            _engine.RenderEllipse(_x, _y, _radiusX, _radiusY);
        }

        public override void Move(float dx, float dy)
        {
            _x += dx; _y += dy;
            Console.WriteLine($"[Ellipse] Перемещён на ({dx},{dy})");
        }
    }

}
