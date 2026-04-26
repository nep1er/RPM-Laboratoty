using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Services;
using PhoneBook.ViewModels;
using PhoneBook.Views;

namespace PhoneBook
{
    // Точка входа в приложение.
    // Отвечает за настройку контейнера внедрения зависимостей и запуск главного окна.

    public partial class App : Application
    {
        // Переопределённый метод, вызываемый при запуске приложения.
        // Здесь настраивается контейнер зависимостей и инициализируется главное окно.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //1. Создаём коллекцию для регистрации сервисов
            var services = new ServiceCollection();

            //2. Регистрируем сервисы с указанием времени жизни (Lifetime)
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var window = new MainWindow();
                window.DataContext = serviceProvider.GetRequiredService<MainViewModel>();
                return window;
            });

            //3. Создаём контейнер зависимостей
            var serviceProvider = services.BuildServiceProvider();

            //4. Получаем главное окно из контейнера и отображаем его
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}