using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    /// <summary>
    /// Интерфейс репозитория контактов с поддержкой асинхронных операций.
    /// </summary>
    public interface IContactRepository
    {
        Task<ObservableCollection<Contact>> GetAllContactsAsync();
        Task<bool> AddContactAsync(Contact contact);
        Task<bool> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(int id);
        Task<bool> ContactWithPhoneExistsAsync(string phone, int? excludeId = null);
    }
}