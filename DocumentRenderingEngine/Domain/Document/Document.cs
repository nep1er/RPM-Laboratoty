using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Domain.Document
{
    public class Document
    {
        private List<Page> _pages = new();
        private IRenderingEngine _engine;

        public Document(IRenderingEngine engine)
        {
            _engine = engine;
        }

        public Page CreatePage()
        {
            var page = new Page();
            _pages.Add(page);
            return page;
        }

        public void RenderAll()
        {
            _engine.BeginRender();
            for (int i = 0; i < _pages.Count; i++)
            {
                Console.WriteLine($"\n=== Страница {i + 1} ===");
                _pages[i].Render();
            }
            _engine.EndRender();
        }
    }
}
