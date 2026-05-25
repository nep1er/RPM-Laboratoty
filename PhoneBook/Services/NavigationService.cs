using Microsoft.Extensions.DependencyInjection;
using PhoneBook.ViewModels;
using System;

namespace PhoneBook.Services
{
    /// <summary>
    /// Реализация сервиса навигации на базе DI-контейнера.
    /// Получает ViewModel из контейнера и уведомляет об изменениях.
    /// </summary>
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object? _currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
        {
            var vm = _serviceProvider.GetRequiredService<TViewModel>();

            if (vm is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(parameter);
            }

            CurrentViewModel = vm;
        }
    }
}