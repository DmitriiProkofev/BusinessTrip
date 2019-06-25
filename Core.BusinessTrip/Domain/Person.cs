using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    [DataContract]
    [Table("Persons")]
    public class Person
    {
        #region Constructors

        public Person()
        {
        }

        public Person(string name, string personnelNumber, int positionId, int departmentId, string phoneNumber)
        {
            Name = name;
            PersonnelNumber = personnelNumber;
            PositionId = positionId;
            DepartmentId = departmentId;
            PhoneNumber = phoneNumber;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// ФИО.
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Табельный номер.
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        [Required]
        public string PersonnelNumber { get; set; }

        ///// <summary>
        ///// Должность.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //[Required]
        //public string Position { get; set; }

        ///// <summary>
        ///// Отдел.
        ///// </summary>
        //[DataMember]
        //[MaxLength(100)]
        //public string Department { get; set; }

        ///// <summary>
        ///// Адрес.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //public string Address { get; set; }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        [DataMember]
        public int? LocationId { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        [DataMember]
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        /// <summary>
        /// Ид руководителя.
        /// </summary>
        [DataMember]
        public int? HeadId { get; set; }

        /// <summary>
        /// Руководитель.
        /// </summary>
        [DataMember]
        [ForeignKey("HeadId")]
        public virtual Person Head { get; set; }


        ///// <summary>
        ///// Начальник.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //public string Head { get; set; }

        /// <summary>
        /// Телефонный номер.
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Партии - сотрудники.
        /// </summary>
        [DataMember]
        public virtual List<PartyPerson> PartyPersons { get; set; }

        /// <summary>
        /// Ид должности.
        /// </summary>
        [DataMember]
        public int PositionId { get; set; }

        /// <summary>
        /// Должность.
        /// </summary>
        [DataMember]
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }

        [NotMapped]
        public string PositionToString
        {
            get { return Position.Nominative; }
        }

        /// <summary>
        /// Ид отдела.
        /// </summary>
        [DataMember]
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Отдел.
        /// </summary>
        [DataMember]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [NotMapped]
        public string DepartmentToString
        {
            get { return Department != null ? Department.Nominative : ""; }
        }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion //Override Methods
    }
}
