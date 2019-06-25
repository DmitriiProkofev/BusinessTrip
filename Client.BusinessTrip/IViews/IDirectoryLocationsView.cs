using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфес представления "Справочник местоположений".
    /// </summary>
    public interface IDirectoryLocationsView : IBaseView<Location>
    {
        /// <summary>
        /// Местоположения
        /// </summary>
        List<Location> Locations { get; set; }

        /// <summary>
        /// Текущее выбранное местоположение.
        /// </summary>
        Location CurrentLocation { get; set; }
    }
}
