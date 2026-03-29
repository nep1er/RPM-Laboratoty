using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine
{
    public class PrintRenderer : IRenderingEngine
    {
        public void BeginRender() => Console.WriteLine("[Print] Начало печати");
        public void EndRender() => Console.WriteLine("[Print] Конец печати");

        public void RenderRectangle(float x, float y, float width, float height) =>
            Console.WriteLine($"[Print] Прямоугольник ({x},{y}) {width}x{height}");

        public void RenderEllipse(float x, float y, float radiusX, float radiusY) =>
            Console.WriteLine($"[Print] Круг ({x},{y}) r={radiusX},{radiusY}");

        public void RenderLine(float x1, float y1, float x2, float y2) =>
            Console.WriteLine($"[Print] Линия ({x1},{y1})→({x2},{y2})");
    }
}
