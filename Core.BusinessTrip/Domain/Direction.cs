using Core.BusinessTrip.Helpers.DomainHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Направление.
    /// </summary>
    [DataContract]
    public class Direction
    {
        #region Constructors

        public Direction()
        {
        }

        public Direction(DateTime? dateBegin, DateTime? dateEnd, string reason, Organization organization, Location location)
        {
            DateBegin = dateBegin;
            DateEnd = dateEnd;
            Reason = reason;
            Organization = organization;
            Location = location;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int DirectionId { get; set; }

        /// <summary>
        /// Дата начала.
        /// </summary>
        [DataMember]
        public DateTime? DateBegin { get; set; }

        /// <summary>
        /// Дата окончания.
        /// </summary>
        [DataMember]
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Основание.
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string Reason { get; set; }

        /// <summary>
        /// Ид организации.
        /// </summary>
        [DataMember]
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Организация.
        /// </summary>
        [DataMember]
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [NotMapped]
        public string OrganizationAsString
        {
            get
            {
                return Organization != null ? Organization.ShortName : "";
            }
        }

        /// <summary>
        /// Ид местоположения.
        /// </summary>
        [DataMember]
        public int? LocationId { get; set; }

        /// <summary>
        /// Местоположение.
        /// </summary>
        [DataMember]
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        [NotMapped]
        public string LocationAsString
        {
            get
            {
                return Location != null ? Location.ShortAddress : "";
            }
        }

        [NotMapped]
        public string DateBeginAsString
        {
            get
            {
                return DateBegin != null ? DateBegin.Value.ToShortDateString() : "";
            }
        }

        [NotMapped]
        public string DateEndAsString
        {
            get
            {
                return DateEnd != null ? DateEnd.Value.ToShortDateString() : "";
            }
        }

        /// <summary>
        /// Ид командировки.
        /// </summary>
        [DataMember]
        public int BusinessTripId { get; set; }

        /// <summary>
        /// Командировка.
        /// </summary>
        [DataMember]
        [ForeignKey("BusinessTripId")]
        public virtual BusinessTrip BusinessTrip { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return string.Format("{0}, {1} на объект {2} с {3} по {4}",
                Location != null ? Location.ShortAddress : "", Organization != null ? Organization.ShortName : "", Reason != null ? Reason : "", DateBeginAsString, DateEndAsString);
        }

        #endregion //Override Methods

        //public int CompareTo(Direction y, DirectionComparer.ComparisonType comparisonMethod)
        //{
        //    switch (comparisonMethod)
        //    {
        //        case DirectionComparer.ComparisonType.DateBegin:
        //        default:
        //            return DateBeginAsString.CompareTo(y.DateBeginAsString);
        //        case DirectionComparer.ComparisonType.DateEnd:
        //            return DateEndAsString.CompareTo(y.DateEndAsString);
        //    }
        //}
    }
}
