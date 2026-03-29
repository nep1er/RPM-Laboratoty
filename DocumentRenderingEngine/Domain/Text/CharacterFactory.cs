using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRenderingEngine.Domain.Text
{
    public class CharacterFactory
    {
        private Dictionary<string, Character> _characters = new();

        public Character GetCharacter(char symbol, string font, int fontSize)
        {
            string key = $"{symbol}_{font}_{fontSize}";

            if (!_characters.ContainsKey(key))
            {
                _characters[key] = new Character(symbol, font, fontSize);
                Console.WriteLine($"[Factory] Создан новый character: {key}");
            }
            else
            {
                Console.WriteLine($"[Factory] Найден в кэше: {key}");
            }
            return _characters[key];
        }

        public int GetCount() => _characters.Count;
    }
}
