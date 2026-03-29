using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRenderingEngine.Engine.Core.Interfaces
{
    public interface IRenderingEngine
    {
        void BeginRender();
        void EndRender();
        void RenderRectangle(float x, float y, float width, float height);
        void RenderEllipse(float x, float y, float radiusX, float radiusY);
        void RenderLine(float x1, float y1, float x2, float y2);
    }
}
