using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Engine.Rendering
{
    public class ScreenRenderer : IRenderingEngine
    {
        public void BeginRender() => Console.WriteLine("[Screen] Начало рендеринга");
        public void EndRender() => Console.WriteLine("[Screen] Конец рендеринга");

        public void RenderRectangle(float x, float y, float width, float height) =>
            Console.WriteLine($"[Screen] Прямоугольник ({x},{y}) {width}x{height}");

        public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
            Console.WriteLine($"[Screen] Круг ({x},{y}) r={radiusX},{radiusY}");

        public void RenderLine(float x1, float y1, float x2, float y2) =>
            Console.WriteLine($"[Screen] Линия ({x1},{y1})→({x2},{y2})");
    }
}
