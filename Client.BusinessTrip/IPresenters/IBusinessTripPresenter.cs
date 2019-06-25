using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;

namespace Client.BusinessTrip.IPresenters
{
    /// <summary>
    /// Интерфейс представителя "Командировка".
    /// </summary>
    public interface IBusinessTripPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IBusinessTripView view, IBusinessTripModel model);
    }
}
