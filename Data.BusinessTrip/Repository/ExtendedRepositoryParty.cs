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
        #region Private Fields

        private string _connectionString;

        #endregion //Private Fields

        #region IExtendedRepositoryParty

        /// <summary>
        /// Сохранение партии.
        /// </summary>
        /// <param name="party">Партия.</param>
        /// <param name="persons">Список сотрудников.</param>
        public Party SaveParty(Party party, List<PartyPerson> persons)
        {
            using (var businessTripContex = new BusinessTripContext(ConnectionString))
            {
                businessTripContex.Partys.Add(party);
                businessTripContex.SaveChanges();

                foreach (var person in persons)
                {
                    businessTripContex.PartysPersons.Add(new PartyPerson
                    {
                        Party_PartyId = party.PartyId,
                        Person_PersonId = person.Person_PersonId,
                        IsResponsible = person.IsResponsible
                    });
                }
                businessTripContex.SaveChanges();

                return party;
            }
        }

        /// <summary>
        /// Обновление партии.
        /// </summary>
        /// <param name="party">Партия.</param>
        /// <param name="addPersons">Новые сотрудники в партии.</param>
        /// <param name="deletePersons">Сотрудники в партии к удалению.</param>
        /// <param name="editPersons">Сотрудники в партии для редактирования.</param>
        public void UpdateParty(Party party, List<PartyPerson> addPersons, List<PartyPerson> deletePersons, List<PartyPerson> editPersons)
        {
            using (var businessTripContex = new BusinessTripContext(ConnectionString))
            {
                businessTripContex.Entry(party).State = EntityState.Modified;

                foreach (var person in deletePersons)
                {
                    businessTripContex.Entry(person).State = EntityState.Deleted;
                    //var partyPerson = new PartyPerson
                    //{
                    //    Party_PartyId = party.PartyId,
                    //    Person_PersonId = personId
                    //};

                    //businessTripContex.Entry(partyPerson).State = EntityState.Deleted;
                }

                foreach (var person in addPersons)
                {
                    businessTripContex.Entry(person).State = EntityState.Added;
                    //var partyPerson = new PartyPerson
                    //{
                    //    Party_PartyId = party.PartyId,
                    //    Person_PersonId = person.Key,
                    //    IsResponsible = person.Value
                    //};

                    //businessTripContex.Entry(partyPerson).State = EntityState.Added;
                }

                foreach (var person in editPersons)
                {
                    businessTripContex.Entry(person).State = EntityState.Modified;
                }

                businessTripContex.SaveChanges();
            }
        }

        #endregion //IExtendedRepositoryParty

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
        public Party Save(Party entity)
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
        public void Update(Party entity)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            businessTripContext.Entry(entity).State = EntityState.Modified;
            businessTripContext.SaveChanges();
        }

        /// <summary>
        /// Удалить объекты.
        /// </summary>
        /// <param name="entities">Объекты.</param>
        public void Delete(List<Party> entities)
        {
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

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
            BusinessTripContext businessTripContext = new BusinessTripContext(ConnectionString);

            return businessTripContext.Set<Party>().ToList();
        }

        #endregion //IRepository
    }
}
