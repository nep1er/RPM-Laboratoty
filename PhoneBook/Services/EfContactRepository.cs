using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    /// <summary>
    /// Реализация репозитория контактов на базе Entity Framework Core.
    /// </summary>
    public class EfContactRepository : IContactRepository
    {
        private readonly PhoneBookDbContext _context;

        public EfContactRepository(PhoneBookDbContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<ObservableCollection<Contact>> GetAllContactsAsync()
        {
            var entities = await _context.Contacts.ToListAsync();
            var contacts = entities.Select(Contact.FromEntity).ToList();
            return new ObservableCollection<Contact>(contacts);
        }

        public async Task<bool> AddContactAsync(Contact contact)
        {
            if (contact == null || !contact.Validate())
                return false;

            if (await ContactWithPhoneExistsAsync(contact.Phone))
                return false;

            var entity = contact.ToEntity();
            _context.Contacts.Add(entity);

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                contact.Id = entity.Id;

            return result;
        }


        public async Task<bool> UpdateContactAsync(Contact contact)
        {
            if (contact == null || contact.Id <= 0 || !contact.Validate())
                return false;

            if (await ContactWithPhoneExistsAsync(contact.Phone, contact.Id))
                return false;

            var entity = await _context.Contacts.FindAsync(contact.Id);
            if (entity == null)
                return false;

            entity.Name = contact.Name;
            entity.Phone = contact.Phone;

            _context.Contacts.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            if (id <= 0)
                return false;

            var entity = await _context.Contacts.FindAsync(id);
            if (entity == null)
                return false;

            _context.Contacts.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ContactWithPhoneExistsAsync(string phone, int? excludeId = null)
        {
            // Нормализуем входной номер
            var normalizedInput = NormalizePhone(phone);

            // Загружаем данные из БД в память для клиентской обработки
            var contacts = await _context.Contacts.ToListAsync();

            // Применяем нормализацию и сравнение в памяти
            return contacts.Any(c =>
                (excludeId == null || c.Id != excludeId) &&
                NormalizePhone(c.Phone) == normalizedInput);
        }

        private static string NormalizePhone(string phone)
        {
            return phone?.Replace(" ", "").Replace("-", "")
                         .Replace("(", "").Replace(")", "")
                         .Replace("+", "") ?? string.Empty;
        }
    }
}