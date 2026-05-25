using System.Collections.ObjectModel;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    /// <summary>
    /// Интерфейс репозитория для хранения и управления контактами.
    /// Позволяет сохранять данные между экземплярами ViewModel.
    /// </summary>
    public interface IContactRepository
    {
        ObservableCollection<Contact> Contacts { get; }

        void AddContact(Contact contact);
        void RemoveContact(Contact contact);
        bool ContactWithPhoneExists(string phone, Contact? exclude = null);
    }
}