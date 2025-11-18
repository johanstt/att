using System;

namespace PhotoStudio
{
    /// <summary>
    /// Фотограф фотостудии.
    /// </summary>
    public class Photographer : Person
    {
        private int _experienceYears;
        private string _specialization;
        private decimal _ratePerHour;

        /// <summary>
        /// Стаж (в годах).
        /// </summary>
        public int ExperienceYears
        {
            get => _experienceYears;
            set => _experienceYears = value < 0 ? 0 : value;
        }

        /// <summary>
        /// Специализация (портрет, свадьба, студийная съемка и т.п.).
        /// </summary>
        public string Specialization
        {
            get => _specialization;
            set => _specialization = value ?? string.Empty;
        }

        /// <summary>
        /// Почасовая ставка фотографа.
        /// </summary>
        public decimal RatePerHour
        {
            get => _ratePerHour;
            set => _ratePerHour = value < 0 ? 0 : value;
        }

        public Photographer()
        {
        }

        public Photographer(int id, string name, string phone, string email,
            int experienceYears, string specialization, decimal ratePerHour)
            : base(id, name, phone, email)
        {
            ExperienceYears = experienceYears;
            Specialization = specialization;
            RatePerHour = ratePerHour;
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", стаж: {ExperienceYears} г., спец.: {Specialization}, ставка: {RatePerHour:C}/ч";
        }
    }
}
