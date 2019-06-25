using Core.BusinessTrip.Context;
using Core.BusinessTrip.DataInterfaces;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.BusinessTrip.Repository
{
    /// <summary>
    /// Класс доступа к данным командировок. 
    /// </summary>
    public class ExtendedRepositoryBusinessTrip : IExtendedRepositoryBusinessTrip
    {
        #region IExtendedRepositoryBusinessTrip

        /// <summary>
        /// Получение направлений по Ид командировки.
        /// </summary>
        /// <param name="businessTripId">Ид командировки.</param>
        /// <returns>Список направлений.</returns>
        public List<Direction> GetDirectionsByBusinessTripId(int businessTripId)
        {
            using (var businessTripContext = new BusinessTripContext())
            {
                return businessTripContext.Directions.Include(l => l.Location)
                    .Include(o => o.Organization).Where(d => d.BusinessTripId == businessTripId).ToList();
            }
        }

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        /// <returns>Ид новой командировки.</returns>
        public int SaveBusinessTrip(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            using (var businessTripContex = new BusinessTripContext())
            {
                businessTripContex.BusinessTrips.Add(businessTrip);
                businessTripContex.SaveChanges();

                return businessTrip.BusinessTripId;
            }
        }

        /// <summary>
        /// Удаление направлений по Ид.
        /// </summary>
        /// <param name="directionIds">Ид направлений.</param>
        public void DeleteDirectionByIds(List<int> directionIds)
        {
            using (var businessTripContex = new BusinessTripContext())
            {
                foreach (var directionId in directionIds)
                {
                    var direction = new Direction
                    {
                        DirectionId = directionId
                    };
                    businessTripContex.Directions.Attach(direction);
                    businessTripContex.Directions.Remove(direction);
                }
                businessTripContex.SaveChanges();
            }
        }

        #endregion //IExtendedRepositoryBusinessTrip

        #region IRepository

        /// <summary>
        /// Сохранить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Save(Core.BusinessTrip.Domain.BusinessTrip entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry(entity).State = EntityState.Added;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Update(Core.BusinessTrip.Domain.BusinessTrip entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry<Core.BusinessTrip.Domain.BusinessTrip>(entity).State = EntityState.Modified;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<Core.BusinessTrip.Domain.BusinessTrip> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            foreach (Core.BusinessTrip.Domain.BusinessTrip entity in entities)
            {
                businessTripContext.Entry<Core.BusinessTrip.Domain.BusinessTrip>(entity).State = EntityState.Deleted;
            }
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Объекты.</returns>
        public List<Core.BusinessTrip.Domain.BusinessTrip> GetAll()
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            return businessTripContext.Set<Core.BusinessTrip.Domain.BusinessTrip>().ToList();
        }

        #endregion //IRepository
    }
}
