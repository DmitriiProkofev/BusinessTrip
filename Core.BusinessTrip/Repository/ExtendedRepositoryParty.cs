using Core.BusinessTrip.DataInterfaces;
using System.Collections.Generic;
using System.Linq;
using Core.BusinessTrip.Domain;
using Core.BusinessTrip.Context;
using System.Data.Entity;

namespace Data.BusinessTrip.Repository
{
    /// <summary>
    /// Класс доступа к данным партий. 
    /// </summary>
    public class ExtendedRepositoryParty : IExtendedRepositoryParty
    {
        #region IExtendedRepositoryParty

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        /// <param name="DirectionIds">Ид направлений.</param>
        public void SaveParty(Party party, List<int> personIds)
        {
            using (var businessTripContex = new BusinessTripContext())
            {
                businessTripContex.Partys.Add(party);
                businessTripContex.SaveChanges();

                foreach (var personId in personIds)
                {
                    businessTripContex.PartysPersons.Add(new PartyPerson
                    {
                        Party_PartyId = party.PartyId,
                        Person_PersonId = personId
                    });
                }
                businessTripContex.SaveChanges();
            }
        }

        /// <summary>
        /// Редактирование командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        /// <param name="AddDirectionIds">Ид новых направлений у командировки.</param>
        /// <param name="DeleteDirectionIds">Ид направлений, которые необходимо удалить.</param>
        public void UpdateParty(Party party, List<int> addPersonIds, List<int> deletePersonIds)
        {
            using (var businessTripContex = new BusinessTripContext())
            {
                businessTripContex.Entry(party).State = EntityState.Modified;

                foreach (var personId in deletePersonIds)
                {
                    var partyPerson = new PartyPerson
                    {
                        Party_PartyId = party.PartyId,
                        Person_PersonId = personId
                    };

                    businessTripContex.Entry(partyPerson).State = EntityState.Deleted;
                }

                foreach (var personId in addPersonIds)
                {
                    var partyPerson = new PartyPerson
                    {
                        Party_PartyId = party.PartyId,
                        Person_PersonId = personId
                    };

                    businessTripContex.Entry(partyPerson).State = EntityState.Added;
                }

                businessTripContex.SaveChanges();
            }
        }

        #endregion //IExtendedRepositoryParty

        #region IRepository

        /// <summary>
        /// Сохранить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Save(Party entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry(entity).State = EntityState.Added;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Изменить объект.
        /// </summary>
        /// <param name="entity">Объект.</param>
        public void Update(Party entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            businessTripContext.Entry(entity).State = EntityState.Modified;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<Party> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            foreach (Party entity in entities)
            {
                businessTripContext.Entry(entity).State = EntityState.Deleted;
            }
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Объекты.</returns>
        public List<Party> GetAll()
        {
            BusinessTripContext businessTripContext = new BusinessTripContext();

            return businessTripContext.Set<Party>().ToList();
        }

        #endregion //IRepository
    }
}
