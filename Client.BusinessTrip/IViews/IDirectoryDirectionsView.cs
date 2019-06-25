using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник направлений".
    /// </summary>
    public interface IDirectoryDirectionsView : IBaseView<Direction>
    {
        /// <summary>
        /// Текущее направление.
        /// </summary>
        Direction CurrentDirection { get; set; }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        int? LocationId { get; set; }

        /// <summary>
        /// Ид организации.
        /// </summary>
        int? OrganizationId { get; set; }
    }
}
