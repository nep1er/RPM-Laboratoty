using DocumentRenderingEngine;
using DocumentRenderingEngine.Engine.Core.Interfaces;
using DocumentRenderingEngine.Engine.Effects;
using DocumentRenderingEngine.Engine.Rendering;
using DocumentRenderingEngine.Domain.Shapes;
using DocumentRenderingEngine.Domain.Document;
using DocumentRenderingEngine.Domain.Text;
using DocumentRenderingEngine.Engine.Media;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("ЭТАП 1: Flyweight (Приспособленец)");
        var factory = new CharacterFactory();
        string text = "Hello Flyweight!";

        Console.WriteLine($"\nРендеринг текста: \"{text}\"\n");
        for (int i = 0; i < text.Length; i++)
        {
            var character = factory.GetCharacter(text[i], "Arial", 14);
            character.Draw(i * 10, 0);
        }

        Console.WriteLine($"\n[Stats] Всего создано уникальных Character: {factory.GetCount()}");

        Console.WriteLine("\nЭТАП 2: Proxy (Заместитель)");
        IImage imageProxy = new ImageProxy("photo_hd.jpg");

        Console.WriteLine("\n[Client] Прокси создан, но изображение не загружено");
        Console.WriteLine("[Client] Вызываем Draw() — только сейчас произойдёт загрузка:\n");
        imageProxy.Draw();

        Console.WriteLine("\n[Client] Повторный вызов Draw() — используем кэшированный объект:");
        imageProxy.Draw();
        Console.WriteLine();


        Console.WriteLine("\nЭТАП 3: Bridge (Мост)");
        var screenEngine = new ScreenRenderer();
        var printEngine = new PrintRenderer();

        var screenRect = new Rectangle(screenEngine, 10, 20, 100, 50);
        var printEllipse = new Ellipse(printEngine, 30, 40, 25, 15);
        var screenLine = new Line(screenEngine, 0, 0, 100, 100);

        Console.WriteLine("\nРендеринг на экран:");
        screenRect.Draw();
        screenLine.Draw();

        Console.WriteLine("\nРендеринг на печать:");
        printEllipse.Draw();

        Console.WriteLine("\nДемонстрация независимости иерархий:");
        var printRect = new Rectangle(printEngine, 5, 5, 80, 40);
        printRect.Draw();
        Console.WriteLine();


        Console.WriteLine("\nЭТАП 4: Decorator (Декоратор)");

        IImage baseImage = new ImageProxy("decorated.png");

        Console.WriteLine("\n1. Изображение с рамкой:");
        var withBorder = new BorderDecorator(baseImage, 3, "red");
        withBorder.Draw();

        Console.WriteLine("\n2. Изображение с тенью:");
        var withShadow = new ShadowDecorator(baseImage, 5, 5, "darkgray");
        withShadow.Draw();

        Console.WriteLine("\n3. Изображение с прозрачностью:");
        var withTransparency = new TransparencyDecorator(baseImage, 0.7f);
        withTransparency.Draw();

        Console.WriteLine("\n4. Комбинация: рамка + тень + прозрачность:");
        var decorated = new TransparencyDecorator(
                           new ShadowDecorator(
                               new BorderDecorator(baseImage, 2, "blue"),
                               3, 3, "black"),
                           0.85f);
        decorated.Draw();
        Console.WriteLine();


        Console.WriteLine("\nИНТЕГРАЦИЯ: Полный документ");

        var doc = new Document(new ScreenRenderer());
        var page1 = doc.CreatePage();

        page1.Add(new Rectangle(screenEngine, 0, 0, 200, 100));
        page1.Add(new Ellipse(screenEngine, 50, 50, 30, 20));

        page1.Add(new ImageProxy("doc_image.jpg"));

        var decoratedShape = new BorderDecorator(
                                new ShadowDecorator(
                                    new Line(screenEngine, 10, 10, 150, 80),
                                    2, 2),
                                1);
        page1.Add(decoratedShape);
        doc.RenderAll();

    }
}