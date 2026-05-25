using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Services;
using PhoneBook.ViewModels;

namespace PhoneBook
{
    /// <summary>
    /// Точка входа в приложение.
    /// Настраивает DI-контейнер с поддержкой навигации.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // 1. Сервисы
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // 2. Репозиторий данных (Singleton — данные должны сохраняться!)
            services.AddSingleton<IContactRepository, ContactRepository>();

            // 3. ViewModel для экранов (Transient — новый экземпляр при навигации)
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();

            // 4. ViewModel оболочки (Singleton)
            services.AddSingleton<MainWindowViewModel>();

            // 5. Главное окно
            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var window = new MainWindow();
                window.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}