using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник должностей". 
    /// </summary>
    public interface IDirectoryPositionsView : IBaseView<Position>
    {
        /// <summary>
        /// Должности.
        /// </summary>
        List<Position> Positions { get; set; }

        /// <summary>
        /// Текущая выбранная должность.
        /// </summary>
        Position CurrentPosition { get; set; }
    }
}
