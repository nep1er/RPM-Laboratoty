using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    /// <summary>
    /// Реализация репозитория контактов
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        public ObservableCollection<Contact> Contacts { get; }

        public ContactRepository()
        {
            Contacts = new ObservableCollection<Contact>();
        }

        public Task<ObservableCollection<Contact>> GetAllContactsAsync()
        {
            return Task.FromResult(Contacts);
        }

        public Task<bool> AddContactAsync(Contact contact)
        {
            if (contact != null && !Contacts.Contains(contact) && !ContactWithPhoneExistsAsync(contact.Phone).Result)
            {
                Contacts.Add(contact);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> UpdateContactAsync(Contact contact)
        {
            var existing = Contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existing != null)
            {
                existing.Name = contact.Name;
                existing.Phone = contact.Phone;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> DeleteContactAsync(int id)
        {
            var contact = Contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                Contacts.Remove(contact);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ContactWithPhoneExistsAsync(string phone, int? excludeId = null)
        {
            var normalizedInput = NormalizePhone(phone);

            var exists = Contacts.Any(c =>
                (excludeId == null || c.Id != excludeId) &&
                NormalizePhone(c.Phone) == normalizedInput);

            return Task.FromResult(exists);
        }

        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "")
                         .Replace("-", "")
                         .Replace("(", "")
                         .Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }
    }
}