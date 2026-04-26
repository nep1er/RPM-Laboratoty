using System;
using PhoneBook.ViewModels;

namespace PhoneBook.Models
{
    // Модель контакта телефонной книги.
    // Хранит данные и обеспечивает их валидацию.
    public class Contact : ObservableObject
    {
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
            set
            {
                if (Set(ref _name, value?.Trim() ?? string.Empty)){ }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (Set(ref _phone, value?.Trim() ?? string.Empty)){ }
            }
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            var cleanPhone = Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (cleanPhone.StartsWith("+7"))
                return cleanPhone.Length == 12 && cleanPhone.Substring(2).All(char.IsDigit);

            return cleanPhone.All(char.IsDigit) && (cleanPhone.Length == 10 || cleanPhone.Length == 11);
        }

        public override string ToString() => $"{Name}: {Phone}";
    }
}