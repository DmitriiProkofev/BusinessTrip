using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Партия сотрудников.
    /// </summary>
    [DataContract]
    public class Party
    {
        #region Constructors

        public Party()
        {
        }

        public Party(string name)
        {
            Name = name;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int PartyId { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        public string Name { get; set; }

        ///// <summary>
        ///// Сотрудники.
        ///// </summary>
        //[DataMember]
        //[InverseProperty("Partys")]
        //public virtual List<Person> Persons { get; set; }

        /// <summary>
        /// Партии - сотрудники.
        /// </summary>
        [DataMember]
        public virtual List<PartyPerson> PartyPersons { get; set; }

        ///// <summary>
        ///// Командировка.
        ///// </summary>
        //[DataMember]
        //public virtual BusinessTrip BusinessTrip { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion //Override Methods
    }
}
