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
        private readonly IDbContextFactory<PhoneBookDbContext> _contextFactory;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<Contact> _contacts = new();
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private string _searchText = string.Empty;
        private Contact? _selectedContact;

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

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SearchCommand { get; }

        public ContactsListViewModel(
            IDbContextFactory<PhoneBookDbContext> contextFactory,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _contextFactory = contextFactory ?? throw new System.ArgumentNullException(nameof(contextFactory));
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

        /// <summary>
        /// READ: Загрузка контактов через короткоживущий контекст.
        /// </summary>
        private async Task LoadContactsAsync()
        {
            try
            {
                // Создаём новый контекст только для этой операции
                using var context = _contextFactory.CreateDbContext();

                // Загружаем данные и сразу материализуем их в память
                var entities = await context.Contacts.ToListAsync();

                // Конвертируем в модели ViewModel
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

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                _ = LoadContactsAsync();
                return;
            }

            var filtered = Contacts
                .Where(c => c.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                           c.Phone.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            Contacts = new ObservableCollection<Contact>(filtered);
        }

        /// <summary>
        /// CREATE: Добавление контакта через новый контекст.
        /// </summary>
        private async Task AddContactAsync()
        {
            if (!CanAddContact())
                return;

            // Проверка на дубликат: создаём отдельный контекст для запроса
            var normalizedPhone = NormalizePhone(Phone);

            using (var checkContext = _contextFactory.CreateDbContext())
            {
                var contacts = await checkContext.Contacts.ToListAsync();
                if (contacts.Any(c => NormalizePhone(c.Phone) == normalizedPhone))
                {
                    _dialogService.ShowWarning("Контакт с таким номером уже существует!", "Дубликат");
                    return;
                }
            }

            var contact = new Contact(Name, Phone);
            if (!contact.Validate())
            {
                _dialogService.ShowError("Проверьте корректность данных.", "Ошибка валидации");
                return;
            }

            try
            {
                // Создаём новый контекст для операции вставки
                using var context = _contextFactory.CreateDbContext();

                var entity = contact.ToEntity();
                context.Contacts.Add(entity);
                await context.SaveChangesAsync();

                // Обновляем UI
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

        /// <summary>
        /// DELETE: Удаление через новый контекст.
        /// </summary>
        private async Task DeleteContactAsync(Contact? contact)
        {
            if (contact == null)
                return;

            if (!_dialogService.ShowConfirmation($"Удалить \"{contact.Name}\"?", "Подтверждение"))
                return;

            try
            {
                using var context = _contextFactory.CreateDbContext();

                // Загружаем сущность в новом контексте для отслеживания
                var entity = await context.Contacts.FindAsync(contact.Id);
                if (entity == null)
                {
                    _dialogService.ShowError("Контакт не найден в базе.", "Ошибка");
                    return;
                }

                context.Contacts.Remove(entity);
                await context.SaveChangesAsync();

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

        private void EditContact(Contact? contact)
        {
            if (contact != null)
            {
                _navigationService.NavigateTo<ContactEditViewModel>(contact);
            }
        }

        private bool CanEditContact(Contact? contact) => contact != null;

        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "").Replace("-", "")
                         .Replace("(", "").Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }
    }
}