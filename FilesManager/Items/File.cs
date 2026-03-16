using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager.Items
{
    public class File : FileSystemItem
    {
        public long Size { get; private set; }
        public byte[] Content { get; private set; }

        public File(string name, long size, byte[]? content = null) : base(name)
        {
            Size = size;
            Content = content ?? Array.Empty<byte>();
        }

        public override long GetSize() => Size;

        public override void Add(FileSystemItem item) =>
            throw new InvalidOperationException("Файл не может содержать дочерние элементы");

        public override void Remove(FileSystemItem item) =>
            throw new InvalidOperationException("Файл не может содержать дочерние элементы");

        public override FileSystemItem? GetChild(int index) =>
            throw new InvalidOperationException("Файл не имеет потомков");

        public override List<FileSystemItem> GetAllChildren() => new();

        public override void PrintStructure(string indent = "")
        {
            Console.WriteLine($"{indent} {Name} ({GetSize()} bytes)");
        }

        public byte[] ReadContent() => Content;
        public void WriteContent(byte[] data)
        {
            Content = data;
            Size = data.Length;
        }
    }
}
