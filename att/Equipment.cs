using System;

namespace PhotoStudio
{
    /// <summary>
    /// Единица оборудования фотостудии.
    /// </summary>
    public class Equipment : IIdentifiable
    {
        private int _id;
        private string _name;
        private string _type;
        private bool _isAvailable;
        private decimal _pricePerHour;

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

        /// <summary>
        /// Тип оборудования (камера, объектив, вспышка и т.п.).
        /// </summary>
        public string Type
        {
            get => _type;
            set => _type = value ?? string.Empty;
        }

        /// <summary>
        /// Доступно ли оборудование для бронирования.
        /// </summary>
        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = value;
        }

        /// <summary>
        /// Стоимость аренды за час.
        /// </summary>
        public decimal PricePerHour
        {
            get => _pricePerHour;
            set => _pricePerHour = value < 0 ? 0 : value;
        }

        public Equipment()
        {
        }

        public Equipment(int id, string name, string type, bool isAvailable, decimal pricePerHour)
        {
            Id = id;
            Name = name;
            Type = type;
            IsAvailable = isAvailable;
            PricePerHour = pricePerHour;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} ({Type}), доступно: {(IsAvailable ? "да" : "нет")}, цена: {PricePerHour:C}/ч";
        }
    }
}
