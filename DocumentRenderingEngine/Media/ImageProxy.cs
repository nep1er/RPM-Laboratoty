using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Engine.Media
{
    public class ImageProxy : IImage
    {
        private string _filename;
        private HighResolutionImage _realImage;

        public ImageProxy(string filename)
        {
            _filename = filename;
            Console.WriteLine($"[Proxy] Создан proxy для {_filename} (ленивая загрузка)");
        }

        private void EnsureLoaded()
        {
            if (_realImage == null)
            {
                Console.WriteLine($"[Proxy] Первая загрузка {_filename}...");
                _realImage = new HighResolutionImage(_filename);
            }
        }

        public void Draw()
        {
            EnsureLoaded();
            _realImage.Draw();
        }

        public int GetWidth()
        {
            EnsureLoaded();
            return _realImage.GetWidth();
        }

        public int GetHeight()
        {
            EnsureLoaded();
            return _realImage.GetHeight();
        }
    }
}
