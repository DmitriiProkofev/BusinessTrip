using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    ///Интерфейс представления "Справочник организаций". 
    /// </summary>
    public interface IDirectoryOrganizationsView : IBaseView<Organization>
    {
        /// <summary>
        /// Организации.
        /// </summary>
        List<Organization> Organizations { get; set; }

        /// <summary>
        /// Текущая выбранная организация.
        /// </summary>
        Organization CurrentOrganization { get; set; }
    }
}
