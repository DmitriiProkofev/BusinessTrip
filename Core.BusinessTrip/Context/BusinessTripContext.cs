using Core.BusinessTrip.Domain;
using System.Data.Entity;

namespace Core.BusinessTrip.Context
{
    /// <summary>
    /// Контекст данных "Командировки".
    /// </summary>
    public class BusinessTripContext : DbContext
    {
        public BusinessTripContext() : base("Server=localhost;Database=BusinessTrips;Trusted_Connection=True;")
        {
        }

        public BusinessTripContext(string connectionString)
            :base(connectionString)
        {
        }

        public BusinessTripContext(string connectionString, bool re) 
            : base(connectionString)
        {
            Database.SetInitializer<BusinessTripContext>(null);
        }

        /// <summary>
        /// Доступ к данным командировок.
        /// </summary>
        public DbSet<Domain.BusinessTrip> BusinessTrips { get; set; }

        /// <summary>
        /// Доступ к данным партий.
        /// </summary>
        public DbSet<Party> Partys { get; set; }

        /// <summary>
        /// Доступ к данным приказов.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// Доступ к данным направлений.
        /// </summary>
        public DbSet<Direction> Directions { get; set; }

        /// <summary>
        /// Доступ к данным заявкам на транспорт.
        /// </summary>
        public DbSet<RequestTransport> RequestTransports { get; set; }

        /// <summary>
        /// Доступ к данным организаций.
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Доступ к данным видам работ.
        /// </summary>
        public DbSet<TypeWork> TypeWorks { get; set; }

        /// <summary>
        /// Доступ к данным сотрудников. 
        /// </summary>
        public DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Доступ к данным партии-сотрудники.
        /// </summary>
        public DbSet<PartyPerson> PartysPersons { get; set; }

        /// <summary>
        /// Доступ к данным должностей. 
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Доступ к данным отделов. 
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>
        /// Доступ к данным транспорта. 
        /// </summary>
        public DbSet<Transport> Transports { get; set; }

    }
}
