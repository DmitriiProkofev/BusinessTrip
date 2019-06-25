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
        #region Private Fields

        private string _connectionString;

        #endregion //Private Fields

        #region IExtendedRepositoryBusinessTrip


        public Dictionary<string, string> GetTargetsByReasons(List<string> reasons)
        {
            using (var businessTripContext = new BusinessTripContext("Server=as21430; Initial Catalog=ContractsPIR; Integrated Security = True;", true))
            {
                if (reasons.Count == 0)
                    return null;

                var sqlString = string.Format("Select [CodeProjectP] AS [Code],[NameProject] AS [Target] From [Table_ContractsPIR] Where [CodeProjectP] in ({0})",
                    string.Join(", ", reasons.Distinct().Select(a => string.Format("'{0}'", a))));

                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

                return businessTripContext.Database.SqlQuery<ContractPIR>(sqlString).ToDictionary(c => c.Code, c => c.Target.TrimStart(' ').TrimEnd(' '));
            }
        }

        /// <summary>
        /// Получение направлений по Ид командировки.
        /// </summary>
        /// <param name="businessTripId">Ид командировки.</param>
        /// <returns>Список направлений.</returns>
        public List<Direction> GetDirectionsByBusinessTripId(int businessTripId)
        {
            using (var businessTripContext = new BusinessTripContext(ConnectionString))
            {
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

                return businessTripContext.Directions.Include(l => l.Location)
                    .Include(o => o.Organization).Where(d => d.BusinessTripId == businessTripId).ToList();
            }
        }

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        public Core.BusinessTrip.Domain.BusinessTrip SaveBusinessTrip(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            using (var businessTripContext = new BusinessTripContext(ConnectionString))
            {
                businessTripContext.BusinessTrips.Add(businessTrip);
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
                businessTripContext.SaveChanges();

                return businessTrip;
            }
        }

        /// <summary>
        /// Удаление направлений по Ид.
        /// </summary>
        /// <param name="directionIds">Ид направлений.</param>
        public void DeleteDirectionByIds(List<int> directionIds)
        {
            using (var businessTripContext = new BusinessTripContext(ConnectionString))
            {
                foreach (var directionId in directionIds)
                {
                    var direction = new Direction
                    {
                        DirectionId = directionId
                    };
                    businessTripContext.Directions.Attach(direction);
                    businessTripContext.Directions.Remove(direction);
                }
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
                businessTripContext.SaveChanges();
            }
        }

        public List<Core.BusinessTrip.Domain.BusinessTrip> GetBusinessTripFullByIds(List<int> businessTripIds)
        {
            using (var businessTripContext = new BusinessTripContext(ConnectionString))
            {
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
                return businessTripContext.BusinessTrips
                    .Include(c => c.Command).ToList();
            }
        }

        public List<PartyPerson> GetPartyPersonsByPartyId(int partyId)
        {
            using (var businessTripContext = new BusinessTripContext(ConnectionString))
            {
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
                return businessTripContext.PartysPersons
                    .Include(p=>p.Party)
                    .Include(p=>p.Person)
                    .Include(p=>p.Person.Position)
                    .Include(p=>p.Person.Department).Where(p => p.Party_PartyId == partyId).ToList();
            }
        }

        #endregion //IExtendedRepositoryBusinessTrip

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
        public Core.BusinessTrip.Domain.BusinessTrip Save(Core.BusinessTrip.Domain.BusinessTrip entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            businessTripContext.Configuration.AutoDetectChangesEnabled = false;
            businessTripContext.Configuration.ValidateOnSaveEnabled = false;

            businessTripContext.Entry(entity).State = EntityState.Added;
            businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            businessTripContext.SaveChanges();

            businessTripContext.Configuration.AutoDetectChangesEnabled = true;
            businessTripContext.Configuration.ValidateOnSaveEnabled = true;

            return entity;
        }

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Update(Core.BusinessTrip.Domain.BusinessTrip entity)
        {
            using (BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString))
            {
                businessTripContext.Configuration.AutoDetectChangesEnabled = false;
                businessTripContext.Configuration.ValidateOnSaveEnabled = false;

                var com = entity.Command;
                com.BusinessTripId = entity.BusinessTripId;
                businessTripContext.Entry<Command>(com).State = EntityState.Modified;
                entity.Command = null;

                var requestTransport = entity.RequestTransport;
                requestTransport.BusinessTripId = entity.BusinessTripId;
                businessTripContext.Entry<RequestTransport>(requestTransport).State = EntityState.Modified;
                entity.RequestTransport = null;

                businessTripContext.Entry<Core.BusinessTrip.Domain.BusinessTrip>(entity).State = EntityState.Modified;
                businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
                businessTripContext.SaveChanges();

                businessTripContext.Configuration.AutoDetectChangesEnabled = true;
                businessTripContext.Configuration.ValidateOnSaveEnabled = true;
            }
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<Core.BusinessTrip.Domain.BusinessTrip> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            foreach (Core.BusinessTrip.Domain.BusinessTrip entity in entities)
            {
                businessTripContext.Entry<Command>(new Command { BusinessTripId = entity.BusinessTripId }).State = EntityState.Deleted;
                businessTripContext.Entry<RequestTransport>(new RequestTransport { BusinessTripId = entity.BusinessTripId }).State = EntityState.Deleted;
                businessTripContext.Entry<Core.BusinessTrip.Domain.BusinessTrip>(entity).State = EntityState.Deleted;
            }
            businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Объекты.</returns>
        public List<Core.BusinessTrip.Domain.BusinessTrip> GetAll()
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            //var ff = businessTripContext.Set<Core.BusinessTrip.Domain.BusinessTrip>().ToList();
            //businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            businessTripContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            return businessTripContext.Set<Core.BusinessTrip.Domain.BusinessTrip>().ToList();
        }

        #endregion //IRepository
    }
}
