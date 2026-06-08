using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Data;
using PhoneBook.Services;
using PhoneBook.ViewModels;

namespace PhoneBook
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            //Регистрация DbContext (Scoped)
            services.AddDbContext<PhoneBookDbContext>(options =>
                options.UseSqlServer(
                    "Data Source=.\\SQLEXPRESS;Initial Catalog=PhoneBookDB;Integrated Security=True;TrustServerCertificate=True",
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));

            //ViewModel — теперь получают DbContext напрямую
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();

            //ViewModel оболочки
            services.AddSingleton<MainWindowViewModel>();

            //Главное окно
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}