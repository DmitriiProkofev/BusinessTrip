using System.Collections.Generic;

namespace Core.BusinessTrip.DataInterfaces
{
    /// <summary>
    /// Интнрфейс доступа к данным объектов.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        /// <summary>
        /// Сохранить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        void Delete(List<TEntity> entities);

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Объекты.</returns>
        List<TEntity> GetAll();

        /// <summary>
        /// Строка подключения.
        /// </summary>
        string ConnectionString { get; set; }
    }
}
