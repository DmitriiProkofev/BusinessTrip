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
        /// <summary>
        /// Сохранить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Save(TEntity entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry(entity).State = EntityState.Added;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Update(TEntity entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry<TEntity>(entity).State = EntityState.Modified;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<TEntity> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

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
            BusinessTripContext businessTripContext = new BusinessTripContext();

            return businessTripContext.Set<TEntity>().ToList();
        }
    }
}
