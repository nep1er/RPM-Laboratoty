using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    public class ContactsListViewModel : ObservableObject, INavigationAware
    {
        private readonly PhoneBookDbContext _context;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<Contact> _contacts = new();
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private string _searchText = string.Empty;
        private Contact? _selectedContact;

        // Коллекция для отображения (фильтруемая)
        public ObservableCollection<Contact> Contacts
        {
            get => _contacts;
            private set => Set(ref _contacts, value);
        }

        // Поля ввода
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

        // Поиск/фильтрация
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (Set(ref _searchText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        // Команды
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SearchCommand { get; }

        public ContactsListViewModel(
            PhoneBookDbContext context,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _dialogService = dialogService ?? throw new System.ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService ?? throw new System.ArgumentNullException(nameof(navigationService));

            AddCommand = new AsyncRelayCommand(AddContactAsync, CanAddContact);
            DeleteCommand = new AsyncRelayCommand<Contact>(DeleteContactAsync, CanDeleteContact);
            EditCommand = new RelayCommand<Contact>(EditContact, CanEditContact);
            SearchCommand = new RelayCommand(ApplyFilter);
        }

        public async void OnNavigatedTo(object? parameter)
        {
            await LoadContactsAsync();
            Name = string.Empty;
            Phone = string.Empty;
            SearchText = string.Empty;
            SelectedContact = null;
        }

        private async Task LoadContactsAsync()
        {
            try
            {
                var entities = await _context.Contacts.ToListAsync();

                var contacts = entities.Select(Contact.FromEntity).ToList();
                Contacts = new ObservableCollection<Contact>(contacts);
            }
            catch (DbUpdateException ex)
            {
                _dialogService.ShowError($"Ошибка чтения: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД");
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowError($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Фильтрация контактов по имени или телефону (на стороне клиента).
        /// </summary>
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Если поиск пустой — перезагружаем все контакты
                _ = LoadContactsAsync();
                return;
            }

            // Фильтрация уже загруженной коллекции
            var filtered = Contacts
                .Where(c => c.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                           c.Phone.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Создаём новую коллекцию для обновления UI
            Contacts = new ObservableCollection<Contact>(filtered);
        }

        /// <summary>
        /// CREATE: Добавление нового контакта в базу данных.
        /// </summary>
        private async Task AddContactAsync()
        {
            if (!CanAddContact())
                return;

            // Проверка на дубликат: загружаем данные в память, затем фильтруем
            var normalizedPhone = NormalizePhone(Phone);

            // 1. Загружаем контакты из БД в память
            var contacts = await _context.Contacts.ToListAsync();

            // 2. Применяем нормализацию и сравнение уже в памяти
            var exists = contacts.Any(c => NormalizePhone(c.Phone) == normalizedPhone);

            if (exists)
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!", "Дубликат");
                return;
            }

            // Валидация
            var contact = new Contact(Name, Phone);
            if (!contact.Validate())
            {
                _dialogService.ShowError("Проверьте корректность данных.", "Ошибка валидации");
                return;
            }

            try
            {
                // 1. Создаём сущность БД
                var entity = contact.ToEntity();

                // 2. Добавляем в DbSet — состояние: Added
                _context.Contacts.Add(entity);

                // 3. Сохраняем изменения — генерируется INSERT
                await _context.SaveChangesAsync();

                // 4. Обновляем локальную коллекцию и интерфейс
                contact.Id = entity.Id;
                Contacts.Add(contact);

                Name = string.Empty;
                Phone = string.Empty;

                _dialogService.ShowInfo($"Контакт \"{contact.Name}\" добавлен.", "Успех");
            }
            catch (DbUpdateException ex)
            {
                _dialogService.ShowError($"Не удалось добавить: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД");
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowError($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private bool CanAddContact() =>
            !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);

        private async Task DeleteContactAsync(Contact? contact)
        {
            if (contact == null)
                return;

            if (!_dialogService.ShowConfirmation($"Удалить \"{contact.Name}\"?", "Подтверждение"))
                return;

            try
            {
                var entity = await _context.Contacts.FindAsync(contact.Id);
                if (entity == null)
                {
                    _dialogService.ShowError("Контакт не найден в базе.", "Ошибка");
                    return;
                }

                _context.Contacts.Remove(entity);

                await _context.SaveChangesAsync();

                // 4. Обновляем UI
                Contacts.Remove(contact);
                SelectedContact = null;

                _dialogService.ShowInfo("Контакт удалён.", "Удалено");
            }
            catch (DbUpdateException ex)
            {
                _dialogService.ShowError($"Не удалось удалить: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД");
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowError($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private bool CanDeleteContact(Contact? contact) => contact != null;

        /// <summary>
        /// Переход к редактированию контакта.
        /// </summary>
        private void EditContact(Contact? contact)
        {
            if (contact != null)
            {
                // Передаём контакт как параметр навигации
                _navigationService.NavigateTo<ContactEditViewModel>(contact);
            }
        }

        private bool CanEditContact(Contact? contact) => contact != null;

        /// <summary>
        /// Нормализация номера для сравнения.
        /// </summary>
        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "").Replace("-", "")
                         .Replace("(", "").Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }
    }
}