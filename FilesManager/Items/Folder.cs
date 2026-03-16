using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager.Items
{
    public class Folder : FileSystemItem
    {
        private List<FileSystemItem> _children = new();

        public Folder(string name) : base(name) { }

        public override long GetSize()
        {
            long total = 0;
            foreach (var child in _children)
            {
                total += child.GetSize();
            }
            return total;
        }

        public override void Add(FileSystemItem item)
        {
            if (item != null && !_children.Contains(item))
            {
                _children.Add(item);
                Console.WriteLine($"Добавлено: {item.Name} в {Name}");
            }
        }

        public override void Remove(FileSystemItem item)
        {
            if (_children.Remove(item))
            {
                Console.WriteLine($"Удалено: {item.Name} из {Name}");
            }
        }

        public override FileSystemItem? GetChild(int index)
        {
            if (index >= 0 && index < _children.Count)
                return _children[index];
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public override List<FileSystemItem> GetAllChildren() => new(_children);

        public bool Contains(string itemName) =>
            _children.Any(c => c.Name == itemName);

        public override void PrintStructure(string indent = "")
        {
            Console.WriteLine($"{indent} {Name} ({GetSize()} bytes)");
            foreach (var child in _children)
            {
                child.PrintStructure(indent + "  ");
            }
        }

        public void DeleteAll()
        {
            foreach (var child in _children.ToList())
            {
                if (child is Folder folder)
                    folder.DeleteAll();
                Remove(child);
            }
        }

        public Folder Clone()
        {
            var clone = new Folder(Name);
            foreach (var child in _children)
            {
                if (child is File file)
                {
                    clone.Add(new File(file.Name, file.Size, file.ReadContent()));
                }
                else if (child is Folder subFolder)
                {
                    clone.Add(subFolder.Clone());
                }
            }
            return clone;
        }
    }
}
