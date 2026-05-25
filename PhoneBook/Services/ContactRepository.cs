using System.Collections.ObjectModel;
using System.Linq;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    /// <summary>
    /// Реализация репозитория контактов с хранением в оперативной памяти.
    /// Зарегистрирован как Singleton для сохранения данных между навигациями.
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        public ObservableCollection<Contact> Contacts { get; }

        public ContactRepository()
        {
            Contacts = new ObservableCollection<Contact>();
        }

        public void AddContact(Contact contact)
        {
            if (contact != null && !Contacts.Contains(contact))
            {
                Contacts.Add(contact);
            }
        }

        public void RemoveContact(Contact contact)
        {
            if (contact != null && Contacts.Contains(contact))
            {
                Contacts.Remove(contact);
            }
        }

        /// <summary>
        /// Нормализует номер телефона для сравнения.
        /// </summary>
        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "")
                         .Replace("-", "")
                         .Replace("(", "")
                         .Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }

        public bool ContactWithPhoneExists(string phone, Contact? exclude = null)
        {
            var normalizedInput = NormalizePhone(phone);

            return Contacts.Any(c =>
                c != exclude &&
                NormalizePhone(c.Phone) == normalizedInput);
        }
    }
}