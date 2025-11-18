using System;

namespace PhotoStudio
{
    /// <summary>
    /// Интерфейс для сущностей, имеющих идентификатор.
    /// </summary>
    public interface IIdentifiable
    {
        int Id { get; }
    }
}


