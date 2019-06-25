using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник организаций".
    /// </summary>
    public class DirectoryOrganizationsPresenter : IDirectoryOrganizationsPresenter
    {
        #region Private Fields

        private IDirectoryOrganizationsView _view;
        private IBaseModel<Organization> _model;

        #endregion //Private Fields

        #region IDirectoryOrganizationsPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryOrganizationsView view, IBaseModel<Organization> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryOrganizationsPresenter

        #region Event Handlers

        private void Save_Handler(Organization organization)
        {
            _view.SaveIndex = _model.Save(organization);
        }

        private void Update_Handler(Organization organization)
        {
            _model.Update(organization);
        }

        private void Delete_Handler(List<Organization> organizations)
        {
            _model.Delete(organizations);
        }

        private void GetAll_Handler()
        {
            _view.Organizations = _model.GetAll();
        }

        private void ViewClosed_Handler()
        {
            _view.Save -= Save_Handler;
            _view.Update -= Update_Handler;
            _view.Delete -= Delete_Handler;
            _view.GetAll -= GetAll_Handler;
            _view.ViewClosed -= ViewClosed_Handler;
        }

        #endregion //Event Handlers
    }
}
