using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;

namespace Client.BusinessTrip.IPresenters
{
    /// <summary>
    /// Интерфейс представителя "Справочник направлений".
    /// </summary>
    public interface IDirectoryDirectionsPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IDirectoryDirectionsView view, IBaseModel<Direction> model);
    }
}
