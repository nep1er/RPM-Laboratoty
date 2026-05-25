namespace PhoneBook.Services
{
    /// <summary>
    /// Интерфейс сервиса навигации между экранами приложения.
    /// Реализует паттерн ViewModel-First навигации.
    /// </summary>
    public interface INavigationService
    {
        object? CurrentViewModel { get; }

        void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class;
    }

    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
    }
}