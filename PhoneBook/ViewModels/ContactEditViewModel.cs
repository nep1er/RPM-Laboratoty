using System.Threading.Tasks;
using System.Windows.Input;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.ViewModels
{
    /// <summary>
    /// ViewModel для экрана редактирования контакта с интеграцией БД.
    /// </summary>
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IContactRepository _contactRepository;

        private Contact? _contact;
        private string _editName = string.Empty;
        private string _editPhone = string.Empty;
        private bool _isLoading;

        public string EditName
        {
            get => _editName;
            set
            {
                if (Set(ref _editName, value))
                {
                    if (_contact != null)
                        _contact.Name = value;
                }
            }
        }

        public string EditPhone
        {
            get => _editPhone;
            set
            {
                if (Set(ref _editPhone, value))
                {
                    if (_contact != null)
                        _contact.Phone = value;
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set => Set(ref _isLoading, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IContactRepository contactRepository)
        {
            _navigationService = navigationService
                ?? throw new System.ArgumentNullException(nameof(navigationService));
            _dialogService = dialogService
                ?? throw new System.ArgumentNullException(nameof(dialogService));
            _contactRepository = contactRepository
                ?? throw new System.ArgumentNullException(nameof(contactRepository));

            SaveCommand = new AsyncRelayCommand(SaveContactAsync);
            CancelCommand = new RelayCommand(CancelEditing);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is Contact contact)
            {
                _contact = contact;
                _editName = contact.Name;
                _editPhone = contact.Phone;
            }
            else
            {
                _dialogService.ShowError("Не удалось загрузить данные контакта", "Ошибка");
                _navigationService.NavigateTo<ContactsListViewModel>();
            }
        }

        private async Task SaveContactAsync()
        {
            if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditPhone))
            {
                _dialogService.ShowWarning("Заполните все поля", "Ошибка ввода");
                return;
            }

            if (_contact != null && !_contact.Validate())
            {
                _dialogService.ShowError(
                    "Неверный формат номера телефона. Используйте формат: +7XXXXXXXXXX",
                    "Ошибка валидации");
                return;
            }

            if (_contact != null &&
                await _contactRepository.ContactWithPhoneExistsAsync(EditPhone, _contact.Id))
            {
                _dialogService.ShowWarning(
                    "Контакт с таким номером телефона уже существует!", "Дубликат");
                return;
            }

            IsLoading = true;
            try
            {
                if (_contact != null)
                {
                    var result = await _contactRepository.UpdateContactAsync(_contact);

                    if (result)
                    {
                        _dialogService.ShowInfo(
                            $"Контакт \"{_contact.Name}\" обновлён", "Успех");
                        _navigationService.NavigateTo<ContactsListViewModel>();
                    }
                    else
                    {
                        _dialogService.ShowError("Не удалось сохранить изменения.", "Ошибка");
                    }
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CancelEditing()
        {
            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}