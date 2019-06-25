using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Местоположение.
    /// </summary>
    [DataContract]
    public class Location
    {
        #region Constructors

        public Location()
        {
        }

        public Location(string address)
        {
            Address = address;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int LocationId { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        public string Address { get; set; }

        /// <summary>
        /// Короткий адрес.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string ShortAddress { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return ShortAddress;
        }

        #endregion //Override Methods
    }
}
