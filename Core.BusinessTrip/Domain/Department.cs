using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Отдел.
    /// </summary>
    [DataContract]
    public class Department
    {
        #region Constructors

        public Department()
        { }

        public Department(string number, string nominative/*, string genitive, string dative*/)
        {
            Number = number;
            Nominative = nominative;
            //Genitive = genitive;
            //Dative = dative;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int DepartmentId { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>
        [DataMember]
        [MaxLength(10)]
        public string Number { get; set; }

        //[DataMember]
        //[MaxLength(10)]
        //public string Group { get; set; }

        /// <summary>
        /// Именительный падеж.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        [Required]
        public string Nominative { get; set; }

        [DataMember]
        [MaxLength(100)]
        [Required]
        public string Category { get; set; }

        ///// <summary>
        ///// Родительный падеж.
        ///// </summary>
        //[DataMember]
        //[MaxLength(250)]
        //[Required]
        //public string Genitive { get; set; }

        ///// <summary>
        ///// Дательный падеж.
        ///// </summary>
        //[DataMember]
        //[MaxLength(250)]
        //[Required]
        //public string Dative { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Nominative;
        }

        #endregion //Override Methods
    }
}
