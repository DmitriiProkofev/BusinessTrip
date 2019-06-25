using Core.BusinessTrip.Helpers.DomainHelpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Партия-сотрудник.
    /// </summary>
    [DataContract]
    public class PartyPerson
    {
        /// <summary>
        /// Ид партии.
        /// </summary>
        [DataMember]
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Party")]
        public int Party_PartyId { get; set; }

        /// <summary>
        /// Ид сотрудника.
        /// </summary>
        [DataMember]
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Person")]
        public int Person_PersonId { get; set; }

        /// <summary>
        /// Партия.
        /// </summary>
        [DataMember]
        public virtual Party Party { get; set; }

        /// <summary>
        /// Сотрудник.
        /// </summary>
        [DataMember]
        public virtual Person Person { get; set; }

        /// <summary>
        /// Ответственный в партии.
        /// </summary>
        [DataMember]
        [Column(Order = 3)]
        public bool IsResponsible { get; set; }

        [NotMapped]
        public string Name
        {
            get { return Person.Name; }
        }

        [NotMapped]
        public string Position
        {
            get { return Person.Position != null ? Person.Position.Nominative : ""; }
        }

        [NotMapped]
        public string Department
        {
            get { return Person.Department != null ? Person.Department.Nominative : ""; }
        }

        [NotMapped]
        public string PersonnelNumber
        {
            get { return Person.PersonnelNumber; }
        }

        //public int CompareTo(PartyPerson p2, PartyPersonComparer.ComparisonType comparisonMethod)
        //{
        //    switch (comparisonMethod)
        //    {
        //        case PartyPersonComparer.ComparisonType.Name:
        //        default:
        //            return Name.CompareTo(p2.Name);
        //    }
        //}
    }
}
