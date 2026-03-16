using FilesManager.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager
{
    public class SyncFacade
    {
        private readonly IFileSystem _sourceFS;
        private readonly IFileSystem _targetFS;
        private readonly string _sourcePrefix;
        private readonly string _targetPrefix;

        public SyncFacade(IFileSystem source, IFileSystem target,
                         string sourcePrefix = "Source", string targetPrefix = "Target")
        {
            _sourceFS = source;
            _targetFS = target;
            _sourcePrefix = sourcePrefix;
            _targetPrefix = targetPrefix;
        }

        public void SyncFolder(string sourcePath, string targetPath)
        {
            Console.WriteLine($"\nСинхронизация...: {sourcePath} → {targetPath}");

            try
            {
                var items = _sourceFS.ListItems(sourcePath);
                int synced = 0;

                foreach (var item in items)
                {
                    //извлекаем имя файла из строки "[FILE] name (size bytes)"
                    var itemName = ExtractName(item);
                    if (string.IsNullOrEmpty(itemName)) continue;

                    var sourceFilePath = $"{sourcePath}/{itemName}".Replace("//", "/");
                    var targetFilePath = $"{targetPath}/{itemName}".Replace("//", "/");

                    try
                    {
                        // Читаем из источника
                        var data = _sourceFS.ReadFile(sourceFilePath);

                        // Записываем в цель
                        _targetFS.WriteFile(targetFilePath, data);
                        synced++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка синхронизации {itemName}: {ex.Message}!");
                    }
                }

                Console.WriteLine($"Синхронизация завершена. Обработано файлов: {synced}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка синхронизации: {ex.Message}!");
                throw;
            }
        }

        public void Backup(string sourcePath, string backupPath)
        {
            Console.WriteLine($"\nРезервное копирование: {sourcePath} >> {backupPath}");
            var log = new List<string>();

            try
            {
                var items = _sourceFS.ListItems(sourcePath);

                foreach (var item in items)
                {
                    var itemName = ExtractName(item);
                    if (string.IsNullOrEmpty(itemName)) continue;

                    var isFile = item.Contains("[FILE]");
                    var sourceFilePath = $"{sourcePath}/{itemName}".Replace("//", "/");
                    var backupFilePath = $"{backupPath}/{itemName}".Replace("//", "/");

                    try
                    {
                        if (isFile)
                        {
                            var data = _sourceFS.ReadFile(sourceFilePath);
                            _targetFS.WriteFile(backupFilePath, data);
                            log.Add($"{itemName}");
                        }
                        else
                        {
                            // Рекурсивный бэкап подпапок
                            Backup(sourceFilePath, backupFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Add($"{itemName}: {ex.Message}");
                        Console.WriteLine($"Не удалось создать бэкап {itemName}");
                    }
                }

                Console.WriteLine("\nОтчёт о резервном копировании:");
                foreach (var entry in log)
                {
                    Console.WriteLine($"   {entry}");
                }
                Console.WriteLine($"Резервное копирование завершено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании резервной копии: {ex.Message}");
                throw;
            }
        }

        private string ExtractName(string listItem)
        {
            //формат: "[FILE] name (123 bytes)" или "[DIR]  name (0 bytes)"
            var start = listItem.IndexOf(']') + 2;
            if (start < 2) return null;

            var end = listItem.LastIndexOf('(');
            if (end < 0) return listItem.Substring(start).Trim();

            return listItem.Substring(start, end - start).Trim();
        }

        public bool VerifySync(string sourcePath, string targetPath)
        {
            Console.WriteLine($"\nПроверка целостности: {sourcePath} ↔ {targetPath}");

            var sourceItems = _sourceFS.ListItems(sourcePath);
            var targetItems = _targetFS.ListItems(targetPath);

            var sourceNames = sourceItems.Select(ExtractName).Where(n => n != null).ToHashSet();
            var targetNames = targetItems.Select(ExtractName).Where(n => n != null).ToHashSet();

            var missing = sourceNames.Except(targetNames);
            var extra = targetNames.Except(sourceNames);

            if (!missing.Any() && !extra.Any())
            {
                Console.WriteLine("Синхронизация подтверждена: все файлы на месте");
                return true;
            }

            if (missing.Any())
                Console.WriteLine($"Отсутствуют в цели: {string.Join(", ", missing)}");
            if (extra.Any())
                Console.WriteLine($"Лишние файлы в цели: {string.Join(", ", extra)}");

            return false;
        }
    }
}
