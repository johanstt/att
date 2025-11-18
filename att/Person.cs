using System;

namespace PhotoStudio
{
    /// <summary>
    /// Абстрактный человек (клиент, фотограф и т.д.).
    /// </summary>
    public abstract class Person : IIdentifiable
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _email;

        public int Id
        {
            get => _id;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id должен быть положительным.");
                _id = value;
            }
        }

        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value ?? string.Empty;
        }

        public string Email
        {
            get => _email;
            set => _email = value ?? string.Empty;
        }

        protected Person()
        {
        }

        protected Person(int id, string name, string phone, string email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}, тел.: {Phone}, email: {Email}";
        }
    }
}


