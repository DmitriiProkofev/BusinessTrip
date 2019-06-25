using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;

namespace Client.BusinessTrip.IPresenters
{
    /// <summary>
    /// Интерфейс представителя "Справочник организаций".
    /// </summary>
    public interface IDirectoryOrganizationsPresenter
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        void Init(IDirectoryOrganizationsView view, IBaseModel<Organization> model);
    }
}
