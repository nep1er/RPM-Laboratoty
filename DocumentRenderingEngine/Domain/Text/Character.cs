using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRenderingEngine.Domain.Text
{
    public class Character
    {
        private char _symbol;
        private string _font;
        private int _fontSize;

        public Character(char symbol, string font, int fontSize)
        {
            _symbol = symbol;
            _font = font;
            _fontSize = fontSize;
            Console.WriteLine($"Создан Character: '{_symbol}' ({_font}, {_fontSize})");
        }

        public void Draw(int positionX, int positionY)
        {
            Console.WriteLine($"  Символ '{_symbol}' шрифт:{_font} размер:{_fontSize} " +
                             $"позиция:({positionX},{positionY})");
        }
    }
}
