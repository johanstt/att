using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoStudio
{
    /// <summary>
    /// Фотосессия, включающая клиента, фотографа и оборудование.
    /// </summary>
    public class PhotoSession : IIdentifiable
    {
        private int _id;
        private Client _client;
        private Photographer _photographer;
        private List<Equipment> _usedEquipment;
        private DateTime _date;
        private int _durationMinutes;
        private string _location;
        private decimal _totalPrice;

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

        public Client Client
        {
            get => _client;
            set => _client = value ?? throw new ArgumentNullException(nameof(Client));
        }

        public Photographer Photographer
        {
            get => _photographer;
            set => _photographer = value ?? throw new ArgumentNullException(nameof(Photographer));
        }

        public List<Equipment> UsedEquipment
        {
            get => _usedEquipment;
            set => _usedEquipment = value ?? new List<Equipment>();
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public int DurationMinutes
        {
            get => _durationMinutes;
            set => _durationMinutes = value < 0 ? 0 : value;
        }

        public string Location
        {
            get => _location;
            set => _location = value ?? string.Empty;
        }

        /// <summary>
        /// Итоговая стоимость фотосессии.
        /// </summary>
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => _totalPrice = value < 0 ? 0 : value;
        }

        public PhotoSession()
        {
            UsedEquipment = new List<Equipment>();
        }

        public PhotoSession(int id, Client client, Photographer photographer,
            List<Equipment> usedEquipment, DateTime date, int durationMinutes,
            string location, decimal totalPrice)
        {
            Id = id;
            Client = client;
            Photographer = photographer;
            UsedEquipment = usedEquipment ?? new List<Equipment>();
            Date = date;
            DurationMinutes = durationMinutes;
            Location = location;
            TotalPrice = totalPrice;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Фотосессия [{Id}]");
            sb.AppendLine($"  Дата: {Date:g}, длительность: {DurationMinutes} мин, место: {Location}");
            sb.AppendLine($"  Клиент: {Client.Name} (Id={Client.Id})");
            sb.AppendLine($"  Фотограф: {Photographer.Name} (Id={Photographer.Id})");
            sb.Append("  Оборудование: ");
            if (UsedEquipment.Count == 0)
            {
                sb.AppendLine("нет");
            }
            else
            {
                for (int i = 0; i < UsedEquipment.Count; i++)
                {
                    var e = UsedEquipment[i];
                    sb.Append($"{e.Name} (Id={e.Id})");
                    if (i < UsedEquipment.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine();
            }

            sb.AppendLine($"  Итоговая стоимость: {TotalPrice:C}");
            return sb.ToString();
        }
    }
}
