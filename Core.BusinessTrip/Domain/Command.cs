using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Приказ.
    /// </summary>
    [DataContract]
    public class Command
    {
        #region Constructors

        public Command()
        {
        }

        public Command(BusinessTrip businessTripOf, Person responsibleForEquipment, Person responsibleForData, Person responsibleForCommunication,
            Person responsibleForTechnicalSecurity, Person responsibleForFireSecurity, Person responsibleForIndustrialSecurity, Person responsibleForRigInspection,
            /*DateTime? dateBegin, DateTime? dateEnd,*/ Person responsibleForDealer, DateTime? dateDelivery, Person responsibleForReceiving,
            Person responsibleForMonitoring, Person responsibleForInformation)
        {
            BusinessTripOf = businessTripOf;
            ResponsibleForEquipment = responsibleForEquipment;
            ResponsibleForData = responsibleForData;
            ResponsibleForCommunication = responsibleForCommunication;
            ResponsibleForTechnicalSecurity = responsibleForTechnicalSecurity;
            ResponsibleForFireSecurity = responsibleForFireSecurity;
            ResponsibleForIndustrialSecurity = responsibleForIndustrialSecurity;
            ResponsibleForRigInspection = responsibleForRigInspection;
            //DateBegin = dateBegin;
            //DateEnd = dateEnd;
            ResponsibleForDealer = responsibleForDealer;
            DateDelivery = dateDelivery;
            ResponsibleForReceiving = responsibleForReceiving;
            ResponsibleForMonitoring = responsibleForMonitoring;
            ResponsibleForInformation = responsibleForInformation;
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

        /// <summary>
        /// Ид ответственного за оборудование.
        /// </summary>
        [DataMember]
        public int? ResponsibleForEquipmentId { get; set; }

        /// <summary>
        /// Ответственный за оборудование.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForEquipmentId")]
        public virtual Person ResponsibleForEquipment { get; set; }

        /// <summary>
        /// Ид ответственного за данные. 
        /// </summary>
        [DataMember]
        public int? ResponsibleForDataId { get; set; }

        /// <summary>
        /// Ответственный за данные.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForDataId")]
        public virtual Person ResponsibleForData { get; set; }

        /// <summary>
        /// Ид ответственного за связь.
        /// </summary>
        [DataMember]
        public int? ResponsibleForCommunicationId { get; set; }

        /// <summary>
        /// Ответственный за связь.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForCommunicationId")]
        public virtual Person ResponsibleForCommunication { get; set; }

        /// <summary>
        /// Ид ответственного за техническую безопасность.
        /// </summary>
        [DataMember]
        public int? ResponsibleForTechnicalSecurityId { get; set; }

        /// <summary>
        /// Ответственный за техническую безопасность.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForTechnicalSecurityId")]
        public virtual Person ResponsibleForTechnicalSecurity { get; set; }

        /// <summary>
        /// Ид ответственного за пожарную безопасность.
        /// </summary>
        [DataMember]
        public int? ResponsibleForFireSecurityId { get; set; }

        /// <summary>
        /// Ответственный за пожарную безопасность.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForFireSecurityId")]
        public virtual Person ResponsibleForFireSecurity { get; set; }

        /// <summary>
        /// Ид ответственного за промышленную безопасность.
        /// </summary>
        [DataMember]
        public int? ResponsibleForIndustrialSecurityId { get; set; }

        /// <summary>
        /// Ответственный за промышленную безопасность.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForIndustrialSecurityId")]
        public virtual Person ResponsibleForIndustrialSecurity { get; set; }

        /// <summary>
        /// Ид ответственного за осмотр буровой.
        /// </summary>
        [DataMember]
        public int? ResponsibleForRigInspectionId { get; set; }

        /// <summary>
        /// Ответственный за осмотр буровой.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForRigInspectionId")]
        public virtual Person ResponsibleForRigInspection { get; set; }

        ///// <summary>
        ///// Дата начала.
        ///// </summary>
        //[DataMember]
        //public DateTime? DateBegin { get; set; }

        ///// <summary>
        ///// Дата окончания.
        ///// </summary>
        //[DataMember]
        //public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Ид ответственного за сдачу материала.
        /// </summary>
        [DataMember]
        public int? ResponsibleForDealerId { get; set; }

        /// <summary>
        /// Ответственный за сдачу материала.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForDealerId")]
        public virtual Person ResponsibleForDealer { get; set; }

        /// <summary>
        /// Дата сдачи материала.
        /// </summary>
        [DataMember]
        public DateTime? DateDelivery { get; set; }

        /// <summary>
        /// Ид ответственного за прием материала.
        /// </summary>
        [DataMember]
        public int? ResponsibleForReceivingId { get; set; }

        /// <summary>
        /// Ответственный за прием материала.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForReceivingId")]
        public virtual Person ResponsibleForReceiving { get; set; }

        /// <summary>
        /// Ид ответственного за контроль выполнения приказа.
        /// </summary>
        [DataMember]
        public int? ResponsibleForMonitoringId { get; set; }

        /// <summary>
        /// Ответственный за контроль выполнения приказа.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForMonitoringId")]
        public virtual Person ResponsibleForMonitoring { get; set; }

        /// <summary>
        /// Ид ответственного за оповещение.
        /// </summary>
        [DataMember]
        public int? ResponsibleForInformationId { get; set; }

        /// <summary>
        /// Ответственный за оповещение.
        /// </summary>
        [DataMember]
        [ForeignKey("ResponsibleForInformationId")]
        public virtual Person ResponsibleForInformation { get; set; }

        #endregion //Data Properties

        #region Override Methods

        #endregion //Override Methods
    }
}
