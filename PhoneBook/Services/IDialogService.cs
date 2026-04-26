namespace PhoneBook.Services
{
    // Интерфейс сервиса для отображения диалоговых окон.
    // Позволяет абстрагировать ViewModel от конкретных реализаций UI.

    public interface IDialogService
    {
        void ShowInfo(string message, string title = "Информация");
        void ShowWarning(string message, string title = "Предупреждение");
        void ShowError(string message, string title = "Ошибка");

        bool ShowConfirmation(string message, string title = "Подтверждение");
    }
}