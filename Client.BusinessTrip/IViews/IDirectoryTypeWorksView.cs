using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник видов работ"
    /// </summary>
    public interface IDirectoryTypeWorksView : IBaseView<TypeWork>
    {
        /// <summary>
        /// Виды работ.
        /// </summary>
        List<TypeWork> TypeWorks { get; set; }

        /// <summary>
        /// Текущий выбранный вид работ.
        /// </summary>
        TypeWork CurrentTypeWork { get; set; }
    }
}
