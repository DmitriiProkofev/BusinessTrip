using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Командировка.
    /// </summary>
    [DataContract]
    public class BusinessTrip
    {
        #region Constructors

        public BusinessTrip()
        {
        }

        public BusinessTrip(string numberDocument,DateTime dateFormulation, string target)
        {
            NumberDocument = numberDocument;
            DateFormulation = dateFormulation;
            Target = target;
        }

        public BusinessTrip(Party party, Command command, RequestTransport requestTransport, TypeWork typeWork, string numberDocument,
            DateTime dateFormulation, string target, Person headStructuralDivision, Person headOrganization, Person authored)
        {
            Party = party;
            Command = command;
            RequestTransport = requestTransport;
            //Directions = directions;
            TypeWork = typeWork;
            NumberDocument = numberDocument;
            DateFormulation = dateFormulation;
            Target = target;
            HeadStructuralDivision = headStructuralDivision;
            HeadOrganization = headOrganization;
            Authored = authored;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int BusinessTripId { get; set; }

        /// <summary>
        /// Заявка на транспорт.
        /// </summary>
        [DataMember]
        public virtual RequestTransport RequestTransport { get; set; }

        /// <summary>
        /// Ид партии. 
        /// </summary>
        [DataMember]
        [ForeignKey("Party")]
        public int? PartyId { get; set; }

        /// <summary>
        /// Партия.
        /// </summary>
        [DataMember]
        public virtual Party Party { get; set; }

        /// <summary>
        /// Направления.
        /// </summary>
        //[DataMember]
        //[InverseProperty("BusinessTrips")]
        //public virtual List<Direction> Directions { get; set; }

        /// <summary>
        /// Ид вида работ.
        /// </summary>
        [DataMember]
        public int? TypeWorkId { get; set; }

        /// <summary>
        /// Вид работ.
        /// </summary>
        [DataMember]
        [ForeignKey("TypeWorkId")]
        public virtual TypeWork TypeWork { get; set; }

        /// <summary>
        /// Приказ.
        /// </summary>
        [DataMember]
        public virtual Command Command { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string NumberDocument { get; set; }

        /// <summary>
        /// Дата составления командировки.
        /// </summary>
        [DataMember]
        public DateTime DateFormulation { get; set; }

        /// <summary>
        /// Цель.
        /// </summary>
        [DataMember]
        [MaxLength(2000)]
        public string Target { get; set; }

        /// <summary>
        /// Ид руководителя структурного подразделения.
        /// </summary>
        [DataMember]
        public int? HeadStructuralDivisionId { get; set; }

        /// <summary>
        /// Руководитель структурного подразделения.
        /// </summary>
        [DataMember]
        [ForeignKey("HeadStructuralDivisionId")]
        public virtual Person HeadStructuralDivision { get; set; }

        /// <summary>
        /// Доверенность для руководителя структурного подразделения.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        public string ProxyHeadSD { get; set; }

        /// <summary>
        /// Ид главы организации. 
        /// </summary>
        [DataMember]
        public int? HeadOrganizationId { get; set; }

        /// <summary>
        /// Глава организации.
        /// </summary>
        [DataMember]
        [ForeignKey("HeadOrganizationId")]
        public virtual Person HeadOrganization { get; set; }

        /// <summary>
        /// Доверенность для руководителя организации.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        public string ProxyHeadO { get; set; }

        /// <summary>
        /// Ид автора отчета.
        /// </summary>
        [DataMember]
        public int? AuthoredId { get; set; }

        /// <summary>
        /// Автор отчета.
        /// </summary>
        [DataMember]
        [ForeignKey("AuthoredId")]
        public virtual Person Authored { get; set; }

        /// <summary>
        /// Признак празднечного дня. 
        /// </summary>
        [DataMember]
        public bool IsHoliday { get; set; }

        /// <summary>
        /// Признак выезда в Оренбург.
        /// </summary>
        [DataMember]
        public bool IsOrenburgWork { get; set; }

        /// <summary>
        /// Направления.
        /// </summary>
        [DataMember]
        public virtual List<Direction> Directions { get; set; }

        [NotMapped]
        public string AuthoredToString
        {
            get
            {
                return Authored != null ? Authored.Name : "";
            }
        }

        [NotMapped]
        public string DateFormulationToString
        {
            get
            {
                return DateFormulation != null ? DateFormulation.ToShortDateString() : "";
            }
        }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return string.Format("Командировка № {0}", NumberDocument);
        }

        #endregion //Override Methods
    }
}
