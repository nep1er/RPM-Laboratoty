using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    /// <summary>
    /// ViewModel для экрана списка контактов с интеграцией базы данных.
    /// </summary>
    public class ContactsListViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IContactRepository _contactRepository;

        private ObservableCollection<Contact> _contacts = new();
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact? _selectedContact;
        private bool _isLoading;

        public ObservableCollection<Contact> Contacts
        {
            get => _contacts;
            private set => Set(ref _contacts, value);
        }

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

        public bool IsLoading
        {
            get => _isLoading;
            private set => Set(ref _isLoading, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditContactCommand { get; }
        public ICommand RefreshCommand { get; }

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

            // Асинхронные команды для работы с БД
            AddCommand = new AsyncRelayCommand(AddContactAsync, CanAddContact);
            DeleteCommand = new AsyncRelayCommand<Contact>(DeleteContactAsync, CanDeleteContact);
            EditContactCommand = new RelayCommand<Contact>(EditContact, CanEditContact);
            RefreshCommand = new AsyncRelayCommand(LoadContactsAsync);
        }

        /// <summary>
        /// Загружает контакты из базы данных при навигации к экрану.
        /// </summary>
        public async void OnNavigatedTo(object? parameter)
        {
            await LoadContactsAsync();
            Name = string.Empty;
            Phone = string.Empty;
            SelectedContact = null;
        }

        /// <summary>
        /// Загрузка контактов из базы данных.
        /// </summary>
        private async Task LoadContactsAsync()
        {
            IsLoading = true;
            try
            {
                Contacts = await _contactRepository.GetAllContactsAsync();
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowError($"Ошибка загрузки: {ex.Message}", "Ошибка");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddContactAsync()
        {
            if (!CanAddContact())
                return;

            if (await _contactRepository.ContactWithPhoneExistsAsync(Phone))
            {
                _dialogService.ShowWarning(
                    "Контакт с таким номером телефона уже существует!", "Дубликат");
                return;
            }

            var contact = new Contact(Name, Phone);

            if (!contact.Validate())
            {
                _dialogService.ShowError(
                    "Проверьте корректность введённых данных.", "Ошибка валидации");
                return;
            }

            var result = await _contactRepository.AddContactAsync(contact);

            if (result)
            {
                // Обновляем локальную коллекцию
                Contacts.Add(contact);

                Name = string.Empty;
                Phone = string.Empty;

                _dialogService.ShowInfo(
                    $"Контакт \"{contact.Name}\" успешно добавлен.", "Успех");
            }
            else
            {
                _dialogService.ShowError("Не удалось добавить контакт.", "Ошибка");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !IsLoading;
        }

        private async Task DeleteContactAsync(Contact? contact)
        {
            if (contact == null)
                return;

            bool confirmed = _dialogService.ShowConfirmation(
                $"Вы действительно хотите удалить контакт \"{contact.Name}\"?",
                "Подтверждение удаления");

            if (!confirmed)
                return;

            var result = await _contactRepository.DeleteContactAsync(contact.Id);

            if (result)
            {
                Contacts.Remove(contact);
                _dialogService.ShowInfo(
                    $"Контакт \"{contact.Name}\" удалён.", "Удалено");
                SelectedContact = null;
            }
            else
            {
                _dialogService.ShowError("Не удалось удалить контакт.", "Ошибка");
            }
        }

        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null && !IsLoading;
        }

        private void EditContact(Contact? contact)
        {
            if (contact == null)
                return;

            _navigationService.NavigateTo<ContactEditViewModel>(contact);
        }

        private bool CanEditContact(Contact? contact)
        {
            return contact != null && !IsLoading;
        }
    }
}