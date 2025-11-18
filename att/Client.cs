using System;

namespace PhotoStudio
{
    /// <summary>
    /// Клиент фотостудии.
    /// </summary>
    public class Client : Person
    {
        private int _loyaltyLevel;
        private string _notes;

        /// <summary>
        /// Уровень лояльности (например, 1–5).
        /// </summary>
        public int LoyaltyLevel
        {
            get => _loyaltyLevel;
            set => _loyaltyLevel = value < 0 ? 0 : value;
        }

        /// <summary>
        /// Дополнительные заметки о клиенте.
        /// </summary>
        public string Notes
        {
            get => _notes;
            set => _notes = value ?? string.Empty;
        }

        public Client()
        {
        }

        public Client(int id, string name, string phone, string email, int loyaltyLevel, string notes)
            : base(id, name, phone, email)
        {
            LoyaltyLevel = loyaltyLevel;
            Notes = notes;
        }

        public override string ToString()
        {
            return base.ToString() + $", лояльность: {LoyaltyLevel}, примечание: {Notes}";
        }
    }
}

