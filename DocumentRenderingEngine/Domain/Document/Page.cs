using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Domain.Document
{
    public class Page
    {
        private List<IDrawable> _drawables = new();

        public void Add(IDrawable drawable) => _drawables.Add(drawable);

        public void Render()
        {
            foreach (var d in _drawables)
            {
                d.Draw();
                Console.WriteLine();
            }
        }
    }

}
