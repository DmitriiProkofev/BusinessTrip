using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;

namespace Client.BusinessTrip.IPresenters
{
    /// <summary>
    /// Интерфейс представителя "Справочник партий".
    /// </summary>
    public interface IDirectoryPartysPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IDirectoryPartysView view, IDirectoryPartysModel model);
    }
}
