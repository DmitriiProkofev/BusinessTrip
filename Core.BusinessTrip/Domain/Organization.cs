using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Организация.
    /// </summary>
    [DataContract]
    public class Organization
    {
        #region Constructors

        public Organization()
        {
        }

        public Organization(string name)
        {
            Name = name;
        }

        public Organization(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// Аббревиатура.
        /// </summary>
        [DataMember]
        [MaxLength(30)]
        public string ShortName { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return ShortName;
        }

        #endregion //Override Methods
    }
}
