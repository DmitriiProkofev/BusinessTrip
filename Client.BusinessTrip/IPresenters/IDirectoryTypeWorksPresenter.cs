using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;

namespace Client.BusinessTrip.IPresenters
{
    /// <summary>
    /// Интерфейс представителя "Справочник видов работ".
    /// </summary>
    public interface IDirectoryTypeWorksPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IDirectoryTypeWorksView view, IBaseModel<TypeWork> model);
    }
}
