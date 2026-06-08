using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly PhoneBookDbContext _context;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private Contact? _contact;
        private string _editName = string.Empty;
        private string _editPhone = string.Empty;
        private bool _isNewContact;

        public string EditName
        {
            get => _editName;
            set
            {
                if (Set(ref _editName, value) && _contact != null)
                    _contact.Name = value;
            }
        }

        public string EditPhone
        {
            get => _editPhone;
            set
            {
                if (Set(ref _editPhone, value) && _contact != null)
                    _contact.Phone = value;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(
            PhoneBookDbContext context,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _dialogService = dialogService ?? throw new System.ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService ?? throw new System.ArgumentNullException(nameof(navigationService));

            SaveCommand = new AsyncRelayCommand(SaveContactAsync);
            CancelCommand = new RelayCommand(CancelEditing);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is Contact contact && contact.Id > 0)
            {
                _contact = contact;
                EditName = contact.Name;
                EditPhone = contact.Phone;
                _isNewContact = false;
            }
            else
            {
                _isNewContact = true;
                _contact = new Contact();
                EditName = string.Empty;
                EditPhone = string.Empty;
            }
        }

        private async Task SaveContactAsync()
        {
            if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditPhone))
            {
                _dialogService.ShowWarning("Заполните все поля.", "Ошибка ввода");
                return;
            }

            if (!ValidatePhone(EditPhone))
            {
                _dialogService.ShowError("Неверный формат номера телефона.", "Ошибка валидации");
                return;
            }

            try
            {
                if (_isNewContact)
                {
                    await CreateNewContactAsync();
                }
                else
                {
                    await UpdateExistingContactAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                if (message.Contains("unique index") || message.Contains("дубликат"))
                {
                    _dialogService.ShowWarning("Контакт с таким номером уже существует.", "Дубликат");
                }
                else
                {
                    _dialogService.ShowError($"Ошибка сохранения: {message}", "Ошибка БД");
                }
            }
            catch (System.Exception ex)
            {
                _dialogService.ShowError($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async Task CreateNewContactAsync()
        {
            var normalizedPhone = NormalizePhone(EditPhone);
            var contacts = await _context.Contacts.ToListAsync();
            if (contacts.Any(c => NormalizePhone(c.Phone) == normalizedPhone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!", "Дубликат");
                return;
            }

            var entity = new ContactEntity
            {
                Name = EditName.Trim(),
                Phone = EditPhone.Trim()
            };

            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();

            _dialogService.ShowInfo($"Контакт \"{entity.Name}\" создан.", "Успех");
            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        private async Task UpdateExistingContactAsync()
        {
            if (_contact == null || _contact.Id <= 0)
                return;

            var normalizedPhone = NormalizePhone(EditPhone);
            var contacts = await _context.Contacts.ToListAsync();
            if (contacts.Any(c => NormalizePhone(c.Phone) == normalizedPhone && c.Id != _contact.Id))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!", "Дубликат");
                return;
            }

            var entity = await _context.Contacts.FindAsync(_contact.Id);
            if (entity == null)
            {
                _dialogService.ShowError("Контакт не найден.", "Ошибка");
                return;
            }

            entity.Name = EditName.Trim();
            entity.Phone = EditPhone.Trim();

            await _context.SaveChangesAsync();

            _dialogService.ShowInfo($"Контакт \"{entity.Name}\" обновлён.", "Успех");
            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        private void CancelEditing()
        {
            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "").Replace("-", "")
                         .Replace("(", "").Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }

        private bool ValidatePhone(string phone)
        {
            var clean = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            if (clean.StartsWith("+7"))
                return clean.Length == 12 && clean.Substring(2).All(char.IsDigit);
            return clean.All(char.IsDigit) && (clean.Length == 10 || clean.Length == 11);
        }
    }
}