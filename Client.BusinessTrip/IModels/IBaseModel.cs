using System.Collections.Generic;

namespace Client.BusinessTrip.IModels
{
    /// <summary>
    /// Базовый интерфейс модели.
    /// </summary>
    public interface IBaseModel<T> where T : class
    {
        /// <summary>
        /// Сохранение объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        int? Save(T obj);

        /// <summary>
        /// Обновление объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        void Update(T obj);

        /// <summary>
        /// Удаление группы объектов.
        /// </summary>
        /// <param name="persons">Объекты.</param>
        void Delete(List<T> persons);

        /// <summary>
        /// Получение всех объектов.
        /// </summary>
        /// <returns>Список объектов.</returns>
        List<T> GetAll();
    }
}
