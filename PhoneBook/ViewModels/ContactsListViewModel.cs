using PhoneBook.Models;
using PhoneBook.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    /// <summary>
    /// ViewModel для экрана списка контактов.
    /// Использует IContactRepository для сохранения данных между навигациями.
    /// </summary>
    public class ContactsListViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IContactRepository _contactRepository;

        // Поля для ввода новых данных
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact? _selectedContact;

        // Коллекция контактов берётся из репозитория (общая для приложения)
        public ObservableCollection<Contact> Contacts => _contactRepository.Contacts;

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
        public ICommand EditContactCommand { get; }

        public ContactsListViewModel(
            IDialogService dialogService,
            INavigationService navigationService,
            IContactRepository contactRepository)
        {
            _dialogService = dialogService
                ?? throw new System.ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService
                ?? throw new System.ArgumentNullException(nameof(navigationService));
            _contactRepository = contactRepository
                ?? throw new System.ArgumentNullException(nameof(contactRepository));

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanDeleteContact);
            EditContactCommand = new RelayCommand<Contact>(EditContact, CanEditContact);
        }

        /// <summary>
        /// Вызывается при навигации к этому экрану.
        /// Очищает поля ввода для нового контакта.
        /// </summary>
        public void OnNavigatedTo(object? parameter)
        {
            // Сбрасываем поля ввода при возврате к списку
            Name = string.Empty;
            Phone = string.Empty;
            SelectedContact = null;

            // Уведомляем об изменении состояния команд
            CommandManager.InvalidateRequerySuggested();
        }

        private void AddContact()
        {
            if (!CanAddContact())
                return;

            // Проверка дубликата через репозиторий
            if (_contactRepository.ContactWithPhoneExists(Phone))
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

            // Добавление через репозиторий
            _contactRepository.AddContact(contact);

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



            _contactRepository.RemoveContact(contact);

            _dialogService.ShowInfo(
                $"Контакт \"{contact.Name}\" удалён.",
                "Удалено");

            SelectedContact = null;
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null;
        }

        private void EditContact(Contact? contact)
        {
            if (contact == null)
                return;

            // Навигация к экрану редактирования с передачей контакта
            _navigationService.NavigateTo<ContactEditViewModel>(contact);
        }

        private bool CanEditContact(Contact? contact)
        {
            return contact != null;
        }
    }
}