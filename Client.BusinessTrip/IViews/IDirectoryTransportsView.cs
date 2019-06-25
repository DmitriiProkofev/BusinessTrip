using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник транспортных средств". 
    /// </summary>
    public interface IDirectoryTransportsView : IBaseView<Transport>
    {
        /// <summary>
        /// Транспорт.
        /// </summary>
        List<Transport> Transports { get; set; }

        /// <summary>
        /// Текущая выбранный транспорт.
        /// </summary>
        Transport CurrentTransport { get; set; }
    }
}
