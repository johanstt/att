using System.Collections.Generic;

namespace PhotoStudio
{
    /// <summary>
    /// Корневой класс для сериализации всех данных фотостудии.
    /// </summary>
    public class PhotoStudioData
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Photographer> Photographers { get; set; } = new List<Photographer>();
        public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();
        public List<PhotoSession> Sessions { get; set; } = new List<PhotoSession>();
    }
}
