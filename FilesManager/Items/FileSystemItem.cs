using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager.Items
{
    public abstract class FileSystemItem
    {
        public string Name { get; set; }

        protected FileSystemItem(string name)
        {
            Name = name;
        }

        public abstract long GetSize();
        public abstract void Add(FileSystemItem item);
        public abstract void Remove(FileSystemItem item);
        public abstract FileSystemItem? GetChild(int index);
        public abstract List<FileSystemItem> GetAllChildren();

        public FileSystemItem? FindByPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path == Name)
                return this;

            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return FindRecursive(parts, 0);
        }

        private FileSystemItem? FindRecursive(string[] parts, int index)
        {
            if (index >= parts.Length) return this;

            foreach (var child in GetAllChildren())
            {
                if (child.Name == parts[index])
                {
                    if (index == parts.Length - 1)
                        return child;
                    return child.FindRecursive(parts, index + 1);
                }
            }
            return null;
        }

        public abstract void PrintStructure(string indent = "");
    }
}
