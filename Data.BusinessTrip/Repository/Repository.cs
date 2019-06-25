using Core.BusinessTrip.Context;
using Core.BusinessTrip.DataInterfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.BusinessTrip.Repository
{
    /// <summary>
    /// Класс доступа к данным объектов.
    /// </summary>
    /// <typeparam name="TEntity">Объект.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        #region Private Fields

        private string _connectionString;

        #endregion //Private Fields

        #region IRepository

        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// Сохранить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public TEntity Save(TEntity entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            businessTripContext.Entry(entity).State = EntityState.Added;
            businessTripContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Update(TEntity entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            businessTripContext.Entry<TEntity>(entity).State = EntityState.Modified;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<TEntity> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            businessTripContext.Configuration.AutoDetectChangesEnabled = false;
            businessTripContext.Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
            {
                businessTripContext.Entry<TEntity>(entity).State = EntityState.Deleted;   
            }
            businessTripContext.SaveChanges();

            businessTripContext.Configuration.AutoDetectChangesEnabled = true;
            businessTripContext.Configuration.ValidateOnSaveEnabled = true;
        }

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Объекты.</returns>
        public List<TEntity> GetAll()
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            return businessTripContext.Set<TEntity>().ToList();
        }

        #endregion //IRepository
    }
}
