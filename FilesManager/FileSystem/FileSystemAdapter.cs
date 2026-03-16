using FilesManager.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager.FileSystem
{
    public class FileSystemAdapter : IFileSystem
    {
        private readonly FileSystemItem _root;
        private readonly string _systemName;

        public FileSystemAdapter(FileSystemItem root, string systemName = "Local")
        {
            _root = root;
            _systemName = systemName;
        }

        private FileSystemItem? ResolvePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return _root;

            var cleanPath = path.StartsWith($"{_systemName}:/")
                ? path.Substring($"{_systemName}:/".Length)
                : path;

            return _root.FindByPath(cleanPath);
        }

        public List<string> ListItems(string path)
        {
            var item = ResolvePath(path);
            var result = new List<string>();

            if (item is Folder folder)
            {
                foreach (var child in folder.GetAllChildren())
                {
                    var prefix = child is Folder ? "[DIR]  " : "[FILE] ";
                    result.Add($"{prefix}{child.Name} ({child.GetSize()} bytes)");
                }
            }
            else if (item != null)
            {
                result.Add($"[FILE] {item.Name} ({item.GetSize()} bytes)");
            }

            return result;
        }

        public byte[] ReadFile(string path)
        {
            var item = ResolvePath(path);
            if (item is FilesManager.Items.File file)
            {
                Console.WriteLine($"Чтение файла: {path}");
                return file.ReadContent();
            }
            throw new FileNotFoundException($"Файл не найден: {path}");
        }

        public void WriteFile(string path, byte[] data)
        {
            var item = ResolvePath(path);
            if (item is FilesManager.Items.File file)
            {
                file.WriteContent(data);
                Console.WriteLine($"Записано в файл: {path} ({data.Length} bytes)");
                return;
            }

            var lastSlash = path.LastIndexOf('/');
            var fileName = lastSlash >= 0 ? path.Substring(lastSlash + 1) : path;
            var parentPath = lastSlash >= 0 ? path.Substring(0, lastSlash) : "";

            var parent = ResolvePath(parentPath);

            if (parent == null && !string.IsNullOrEmpty(parentPath))
            {
                parent = EnsureFolderExists(parentPath);
            }

            if (parent is Folder folder)
            {
                var newFile = new FilesManager.Items.File(fileName, data.Length, data);
                folder.Add(newFile);
                Console.WriteLine($"Создан новый файл: {path}");
            }
            else
            {
                throw new DirectoryNotFoundException($"Папка не найдена: {parentPath}");
            }
        }

        //Вспомогат метод для создания цепочки папок
        private Folder EnsureFolderExists(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            FileSystemItem current = _root;

            foreach (var part in parts)
            {
                var found = ((Folder)current).GetAllChildren()
                    .FirstOrDefault(c => c.Name == part);

                if (found == null)
                {
                    var newFolder = new Folder(part);
                    ((Folder)current).Add(newFolder);
                    current = newFolder;
                }
                else
                {
                    current = found;
                }
            }

            return (Folder)current;
        }

        public void DeleteItem(string path)
        {
            var item = ResolvePath(path);
            if (item == null || item == _root)
            {
                throw new InvalidOperationException("Нельзя удалить корневой элемент");
            }

            var parent = FindParent(_root, item);
            if (parent is Folder folder)
            {
                folder.Remove(item);
                Console.WriteLine($"Удалено: {path}");
            }
        }

        private FileSystemItem? FindParent(FileSystemItem root, FileSystemItem target)
        {
            if (root is Folder folder)
            {
                foreach (var child in folder.GetAllChildren())
                {
                    if (child == target) return root;
                    if (child is Folder subFolder)
                    {
                        var result = FindParent(subFolder, target);
                        if (result != null) return result;
                    }
                }
            }
            return null;
        }
    }
}
