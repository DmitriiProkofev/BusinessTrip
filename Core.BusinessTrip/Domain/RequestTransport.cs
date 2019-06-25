using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Запрос транспорта.
    /// </summary>
    [DataContract]
    public class RequestTransport
    {
        #region Constructors

        public RequestTransport()
        {
        }

        public RequestTransport(Person projectManager/*, Person transportCustomer*/, Person driver, Location address,
            DateTime? date, double? timeHour, DateTime? dateFormulation)
        {
            ProjectManager = projectManager;
            //TransportCustomer = transportCustomer;
            Driver = driver;
            Address = address;
            Date = date;
            TimeHour = timeHour;
            DateFormulation = dateFormulation;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        [ForeignKey("BusinessTripOf")]
        public int BusinessTripId { get; set; }

        /// <summary>
        /// Командировка.
        /// </summary>
        [DataMember]
        public virtual BusinessTrip BusinessTripOf { get; set; }

        ///// <summary>
        ///// Марка транспорта.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //public string Mark { get; set; }

        /// <summary>
        /// Пробег.
        /// </summary>
        [DataMember]
        [Range(0, int.MaxValue)]
        public double? Mileage { get; set; }

        /// <summary>
        /// Время работы дополнительного оборудования.
        /// </summary>
        [DataMember]
        [Range(0, int.MaxValue)]
        public double? TimeWork { get; set; }

        /// <summary>
        /// Ид сотрудника отдела управления проектами.
        /// </summary>
        [DataMember]
        public int? ProjectManagerID { get; set; }

        /// <summary>
        /// Сотрудник отдела управления проектами.
        /// </summary>
        [DataMember]
        [ForeignKey("ProjectManagerID")]
        public virtual Person ProjectManager { get; set; }

        ///// <summary>
        ///// Ид сотрудника заказывающего транспорт.
        ///// </summary>
        //[DataMember]
        //public int? TransportCustomerID { get; set; }

        ///// <summary>
        ///// Сотрудник заказывающий транспорт.
        ///// </summary>
        //[DataMember]
        //[ForeignKey("TransportCustomerID")]
        //public virtual Person TransportCustomer { get; set; }

        ///// <summary>
        ///// ФИО Водителя.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //public string DriverName { get; set; }

        /// <summary>
        /// Ид водителя.
        /// </summary>
        [DataMember]
        public int? DriverId { get; set; }

        /// <summary>
        /// Водитель.
        /// </summary>
        [DataMember]
        [ForeignKey("DriverId")]
        public virtual Person Driver { get; set; }

        ///// <summary>
        ///// Адресс.
        ///// </summary>
        //[DataMember]
        //[MaxLength(150)]
        //public string Address { get; set; }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        [DataMember]
        public int? AddressId { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        [DataMember]
        [ForeignKey("AddressId")]
        public virtual Location Address { get; set; }


        /// <summary>
        /// Дата.
        /// </summary>
        [DataMember]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Время, час.
        /// </summary>
        [DataMember]
        [Range(0, 100)]
        public double? TimeHour { get; set; }

        /// <summary>
        /// Дата составления. 
        /// </summary>
        [DataMember]
        public DateTime? DateFormulation { get; set; }

        ///// <summary>
        ///// Контактная информация (ФИО\номер телефона).
        ///// </summary>
        //[DataMember]
        //[MaxLength(250)]
        //public string ContactInformation { get; set; }

        /// <summary>
        /// Ид транспорта.
        /// </summary>
        [DataMember]
        [ForeignKey("Transport")]
        public int? TransportId { get; set; }

        /// <summary>
        /// Транспорт.
        /// </summary>
        [DataMember]
        public virtual Transport Transport { get; set; }

        #endregion //Data Properties
    }
}
