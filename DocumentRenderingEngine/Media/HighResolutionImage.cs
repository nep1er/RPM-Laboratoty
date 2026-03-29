using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Engine.Media
{
    public class HighResolutionImage : IImage
    {
        private string _filename;
        private int _width;
        private int _height;
        private bool _isLoaded = false;

        public HighResolutionImage(string filename)
        {
            _filename = filename;
            Console.Write($"[RealImage] Загрузка {_filename}... ");
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            _width = 1920;
            _height = 1080;
            _isLoaded = true;
            Console.WriteLine($"загружено ({_width}x{_height})");
        }

        public void Draw()
        {
            if (!_isLoaded) LoadFromDisk();
            Console.WriteLine($"[RealImage] Отрисовка {_filename}");
        }

        public int GetWidth() => _isLoaded ? _width : 0;
        public int GetHeight() => _isLoaded ? _height : 0;
    }
}
