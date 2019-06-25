using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Транспорт.
    /// </summary>
    [DataContract]
    public class Transport
    {
        #region Constructors

        public Transport()
        { }

        public Transport(string mark)
        {
            Mark = mark;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int TransportId { get; set; }

        /// <summary>
        /// Марка.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string Mark { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Mark;
        }

        #endregion //Override Methods
    }
}
