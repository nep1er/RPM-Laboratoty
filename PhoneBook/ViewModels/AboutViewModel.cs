namespace PhoneBook.ViewModels
{
    /// <summary>
    /// ViewModel для экрана "О программе".
    /// Содержит статическую информацию о приложении.
    /// </summary>
    public class AboutViewModel : ObservableObject
    {
        public string AppName => "Телефонная книга MVVM";

        public string Version => "Версия 0.67.0     ";

        public string Description =>
            "Приложение демонстрирует архитектуру MVVM с внедрением зависимостей и навигацией ViewModel-First.";

        public string Author => "Тимофей Евтифеев";
    }
}