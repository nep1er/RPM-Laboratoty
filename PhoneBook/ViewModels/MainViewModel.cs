using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    //Главная модель представления приложения.
    //Управляет списком контактов и командами пользователя.
    //Зависимости внедряются через конструктор (Constructor Injection).

    public class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;

        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact? _selectedContact;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        //Конструктор с внедрением зависимости через Constructor Injection.
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService
                ?? throw new System.ArgumentNullException(nameof(dialogService));

            Contacts = new ObservableCollection<Contact>();

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanDeleteContact);
        }

        private void AddContact()
        {
            if (!CanAddContact())
                return;

            if (Contacts.Any(c => c.Phone == Phone))
            {
                _dialogService.ShowWarning(
                    "Контакт с таким номером телефона уже существует!",
                    "Дубликат");
                return;
            }

            var contact = new Contact(Name, Phone);

            if (!contact.Validate())
            {
                _dialogService.ShowError(
                    "Проверьте корректность введённых данных.",
                    "Ошибка валидации");
                return;
            }

            Contacts.Add(contact);

            Name = string.Empty;
            Phone = string.Empty;

            _dialogService.ShowInfo(
                $"Контакт \"{contact.Name}\" успешно добавлен.",
                "Успех");

            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        private void DeleteContact(Contact? contact)
        {
            if (contact == null)
                return;

            bool confirmed = _dialogService.ShowConfirmation(
                $"Вы действительно хотите удалить контакт \"{contact.Name}\"?",
                "Подтверждение удаления");

            if (!confirmed)
                return;

            if (Contacts.Contains(contact))
            {
                Contacts.Remove(contact);
                _dialogService.ShowInfo(
                    $"Контакт \"{contact.Name}\" удалён.",
                    "Удалено");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null;
        }
    }
}