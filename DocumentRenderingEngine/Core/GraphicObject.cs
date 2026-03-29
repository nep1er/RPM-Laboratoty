using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Engine.Core
{
    public abstract class GraphicObject : IDrawable
    {
        protected IRenderingEngine _engine;

        public GraphicObject(IRenderingEngine engine)
        {
            _engine = engine;
        }

        public abstract void Draw();
        public abstract void Move(float dx, float dy);
    }
}
