using System.Windows.Input;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна приложения (Shell).
    /// Управляет навигацией между экранами через меню.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public INavigationService NavigationService => _navigationService;

        public ICommand ShowContactsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            // Команда показа экрана контактов
            ShowContactsCommand = new RelayCommand(() =>
                _navigationService.NavigateTo<ContactsListViewModel>());

            // Команда показа экрана "О программе"
            ShowAboutCommand = new RelayCommand(() =>
                _navigationService.NavigateTo<AboutViewModel>());

            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}  