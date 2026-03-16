using FilesManager.FileSystem;
using FilesManager.Items;

namespace FilesManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DemoComposite();
            DemoAdapter();
            DemoFacade();
        }


        static void DemoFacade()
        {
            Console.WriteLine("\n=== ПАТТЕРН FACADE ===\n");

            var localRoot = new Folder("LocalRoot");
            var cloudRoot = new Folder("CloudRoot");

            var localDocs = new Folder("Docs");
            localDocs.Add(new FilesManager.Items.File("report.pdf", 50000, new byte[50000]));
            localDocs.Add(new FilesManager.Items.File("data.csv", 12000, new byte[12000]));
            localRoot.Add(localDocs);

            IFileSystem localFS = new FileSystemAdapter(localRoot, "Local");
            IFileSystem cloudFS = new FileSystemAdapter(cloudRoot, "Cloud");

            var syncFacade = new SyncFacade(localFS, cloudFS, "Local", "Cloud");

            //демонстрация синхронизации
            syncFacade.SyncFolder("Docs", "Backup/Docs");

            //проверка результата
            Console.WriteLine("\nСодержимое облачного хранилища после синхронизации:");
            foreach (var item in cloudFS.ListItems("Backup/Docs"))
            {
                Console.WriteLine($"   {item}");
            }

            //демонстрация резервного копирования
            syncFacade.Backup("Docs", "Archive/Docs_Backup");

            syncFacade.VerifySync("Docs", "Backup/Docs");

            Console.WriteLine(new string('-', 50));
        }

        static void DemoAdapter()
        {
            Console.WriteLine("\n=== ПАТТЕРН ADAPTER ===\n");

            //создаём тестовую структуру
            var fsRoot = new Folder("Storage");
            var dataFolder = new Folder("Data");
            dataFolder.Add(new FilesManager.Items.File("config.json", 512, System.Text.Encoding.UTF8.GetBytes("{\"key\":\"value\"}")));
            dataFolder.Add(new FilesManager.Items.File("log.txt", 1024));
            fsRoot.Add(dataFolder);

            //адаптация под единый интерфейс
            IFileSystem adapter = new FileSystemAdapter(fsRoot, "Local");

            //работа через единый интерфейс
            Console.WriteLine("Список элементов в Data:");
            foreach (var item in adapter.ListItems("Data"))
            {
                Console.WriteLine($"   {item}");
            }

            Console.WriteLine("\nЧтение файла:");
            var content = adapter.ReadFile("Data/config.json");
            Console.WriteLine($"   Содержимое: {System.Text.Encoding.UTF8.GetString(content)}");

            Console.WriteLine("\nЗапись нового файла:");
            adapter.WriteFile("Data/newfile.txt", System.Text.Encoding.UTF8.GetBytes("Hello Adapter!"));

            Console.WriteLine("\nОбновлённый список:");
            foreach (var item in adapter.ListItems("Data"))
            {
                Console.WriteLine($"   {item}");
            }

            Console.WriteLine(new string('-', 50));
        }

        static void DemoComposite()
        {
            Console.WriteLine("=== ПАТТЕРН COMPOSITE ===\n");

            var root = new Folder("Root");

            var docsFolder = new Folder("Documents");
            docsFolder.Add(new FilesManager.Items.File("report.pdf", 102400));
            docsFolder.Add(new FilesManager.Items.File("notes.txt", 2048));

            var imagesFolder = new Folder("Images");
            imagesFolder.Add(new FilesManager.Items.File("photo1.jpg", 2048000));
            imagesFolder.Add(new FilesManager.Items.File("photo2.png", 1536000));

            var projectsFolder = new Folder("Projects");
            var lab5Folder = new Folder("Lab5");
            lab5Folder.Add(new FilesManager.Items.File("program.cs", 15360));
            lab5Folder.Add(new FilesManager.Items.File("readme.md", 4096));
            projectsFolder.Add(lab5Folder);

            root.Add(docsFolder);
            root.Add(imagesFolder);
            root.Add(projectsFolder);

            Console.WriteLine("Структура файловой системы:");
            root.PrintStructure();

            Console.WriteLine($"\nРазмер корневой директории: {root.GetSize():N0} bytes");
            Console.WriteLine($"Размер папки Documents: {docsFolder.GetSize():N0} bytes");

            var found = root.FindByPath("Projects/Lab5/program.cs");
            Console.WriteLine($"\nНайден файл: {found?.Name ?? "не найден"}");

            Console.WriteLine(new string('-', 50));
        }
    }
}
