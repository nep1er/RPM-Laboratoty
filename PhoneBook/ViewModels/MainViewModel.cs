using System.Collections.ObjectModel;
using System.Windows.Input;
using PhoneBook.Models;

namespace PhoneBook.ViewModels
{
    //Главная модель представления приложения.
    //Управляет списком контактов и командами пользователя.

    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Contact> Contacts { get; }

        // Поля для ввода новых данных
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


        public MainViewModel()
        {
            Contacts = new ObservableCollection<Contact>();

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanDeleteContact);
        }

        private void AddContact()
        {
            if (!CanAddContact())
                return;

            var contact = new Contact(Name, Phone);

            if (!contact.Validate())
            {
                System.Windows.MessageBox.Show(
                    "Проверьте корректность введённых данных.",
                    "Ошибка валидации",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            Contacts.Add(contact);

            Name = string.Empty;
            Phone = string.Empty;

            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }
        private void DeleteContact(Contact? contact)
        {
            if (contact != null && Contacts.Contains(contact))
            {
                Contacts.Remove(contact);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null;
        }
    }
}