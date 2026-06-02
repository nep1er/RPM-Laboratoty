using PhoneBook.ViewModels;

namespace PhoneBook.Models
{
    /// <summary>
    /// Модель контакта для использования в ViewModel.
    /// </summary>
    public class Contact : ObservableObject
    {
        public int Id { get; set; }

        private string _name = string.Empty;
        private string _phone = string.Empty;

        public Contact() { }

        public Contact(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value?.Trim() ?? string.Empty);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value?.Trim() ?? string.Empty);
        }

        /// <summary>
        /// Валидация данных контакта.
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            var cleanPhone = Phone.Replace(" ", "").Replace("-", "")
                                  .Replace("(", "").Replace(")", "");

            if (cleanPhone.StartsWith("+7"))
                return cleanPhone.Length == 12 && cleanPhone.Substring(2).All(char.IsDigit);

            return cleanPhone.All(char.IsDigit) && (cleanPhone.Length == 10 || cleanPhone.Length == 11);
        }

        /// <summary>
        /// Конвертация из ContactEntity (EF Core) в Contact (ViewModel).
        /// </summary>
        public static Contact FromEntity(Data.ContactEntity entity)
        {
            return new Contact(entity.Name, entity.Phone) { Id = entity.Id };
        }

        /// <summary>
        /// Конвертация из Contact (ViewModel) в ContactEntity (EF Core).
        /// </summary>
        public Data.ContactEntity ToEntity()
        {
            return new Data.ContactEntity
            {
                Id = Id,
                Name = Name,
                Phone = Phone
            };
        }

        public override string ToString() => $"{Name}: {Phone}";
    }
}