using ComputerConfigurator.Factories;
using System;

namespace ComputerConfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ПАТТЕРН СТРОИТЕЛЬ ===");

            Console.WriteLine("Создание пользовательской конфигурации:");
            var customPC = new ComputerBuilder()
                .WithCPU("AMD Ryzen 9 7950X")
                .WithRAM(64)
                .WithGPU("NVIDIA RTX 4090")
                .WithComponent("Водяное охлаждение")
                .WithComponent("Блок питания 1000W")
                .WithComponent("Корпус с подсветкой")
                .Build();
            customPC.Display();


            Console.WriteLine("\nСоздание предопределённых конфигураций через фабрики:");

            IComputerFactory officeFactory = new OfficeComputerFactory();
            var officePC = officeFactory.CreateComputer();
            Console.WriteLine("Офисный ПК:");
            officePC.Display();

            IComputerFactory gamingFactory = new GamingComputerFactory();
            var gamingPC = gamingFactory.CreateComputer();
            Console.WriteLine("Игровой ПК:");
            gamingPC.Display();

            IComputerFactory homeFactory = new HomeComputerFactory();
            var homePC = homeFactory.CreateComputer();
            Console.WriteLine("Домашний ПК (мультимедиа):");
            homePC.Display();



            Console.WriteLine("\n=== ПАТТЕРН ПРОТОТИП (клонирование) ===");

            var original = new ComputerBuilder()
                .WithCPU("Intel Core i5-12600K")
                .WithRAM(16)
                .WithGPU("NVIDIA GTX 1660")
                .WithComponent("SSD 500GB")
                .WithComponent("Дополнительный вентилятор")
                .Build();

            Console.WriteLine("Оригинальный компьютер:");
            original.Display();

            //Поверхностное копир
            var shallowCopy = original.ShallowCopy();

            //Изменяем копию
            shallowCopy.RAM = 32; //изменяем значение
            shallowCopy.AdditionalComponents.Add("Добавлено в поверхностную"); //изменяем список

            Console.WriteLine("Оригинал после изменений в поверхност копии:");
            original.Display(); //Список изменился, RAM не изменился
            shallowCopy.Display();



            original = new ComputerBuilder()
                .WithCPU("Intel Core i5-12600K")
                .WithRAM(16)
                .WithGPU("NVIDIA GTX 1660")
                .WithComponent("SSD 500GB")
                .WithComponent("Дополнительный вентилятор")
                .Build();


            Console.WriteLine("\n--- Глубокое копирование ---");
            var deepCopy = original.DeepCopy();


            Console.WriteLine("Изменяем глубокую копию...");
            deepCopy.RAM = 32;
            deepCopy.AdditionalComponents.Add("Добавлено в глубокую");

            Console.WriteLine("Оригинал после изменений в глубокой:");
            original.Display(); //ничего не изменилось

            Console.WriteLine("Глубокая копия после изменений:");
            deepCopy.Display();



            Console.WriteLine("\n=== ПАТТЕРН SINGLETON (реестр прототипов) ===");

            PrototypeRegistry registry1 = PrototypeRegistry.Instance;
            PrototypeRegistry registry2 = PrototypeRegistry.Instance;

            //это один ли и тот же объект
            Console.WriteLine($"registry1 и registry2 - один объект? {ReferenceEquals(registry1, registry2)}");

            //создать новый экземпляр через конструктор нельзя (приватный конструктор)
            //PrototypeRegistry registry3 = new PrototypeRegistry(); //Ошибка компиляции

            //все прототипы из реестра
            registry1.DisplayAllPrototypes();

            //копия игрового ПК из реестра
            Console.WriteLine("\n=== Получение и модификация прототипа из реестра ===");

            //глубокую копию
            var myGamingPC = registry1.GetPrototype("gaming");
            Console.WriteLine("Полученный прототип (копия):");
            myGamingPC.Display();


            Console.WriteLine("Модифицируем копию (улучшаем RAM и добавляем компонент)...");
            myGamingPC.RAM = 64;
            myGamingPC.AdditionalComponents.Add("Дополнительный SSD 2TB");
            myGamingPC.AdditionalComponents.Add("Звуковая карта");

            Console.WriteLine("Модифицированная копия:");
            myGamingPC.Display();


            Console.WriteLine("Оригинал в реестре:");
            var originalGaming = registry1.GetOriginalPrototype("gaming");
            originalGaming.Display();



            Console.WriteLine("\n=== Работа с несколькими прототипами ===");

            var officeClone = registry1.GetPrototype("office");
            officeClone.WithComponent("Принтер");
            Console.WriteLine("Офисный ПК с принтером:");
            officeClone.Display();

            var homeClone = registry1.GetPrototype("home");
            homeClone.WithComponent("Антеной");
            Console.WriteLine("Домашний ПК с Антеной:");
            homeClone.Display();


            Console.WriteLine("Проверка оригиналов в реестре:");
            registry1.DisplayAllPrototypes();

        }
    }
}