using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Должность.
    /// </summary>
    [DataContract]
    public class Position
    {
        #region Constructors

        public Position()
        { }

        public Position(string nominative, string genitive, string dative)
        {
            Nominative = nominative;
            Genitive = genitive;
            Dative = dative;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int PositionId { get; set; }

        /// <summary>
        /// Именительный падеж.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string Nominative { get; set; }

        /// <summary>
        /// Родительный падеж.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string Genitive { get; set; }

        /// <summary>
        /// Дательный падеж.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string Dative { get; set; }

        //[DataMember]
        //[MaxLength(100)]
        //[Required]
        //public string Category { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Nominative;
        }

        #endregion //Override Methods
    }
}
