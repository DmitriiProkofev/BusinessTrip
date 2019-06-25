using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;

namespace Client.BusinessTrip.IPresenters
{
    public interface IDirectoryTransportsPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IDirectoryTransportsView view, IBaseModel<Transport> model);
    }
}
