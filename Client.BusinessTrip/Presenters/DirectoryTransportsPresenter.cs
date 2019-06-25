using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник транспорта".
    /// </summary>
    public class DirectoryTransportsPresenter : IDirectoryTransportsPresenter
    {
        #region Private Fields

        private IDirectoryTransportsView _view;
        private IBaseModel<Transport> _model;

        #endregion //Private Fields

        #region IDirectoryOrganizationsPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryTransportsView view, IBaseModel<Transport> model)
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

        private void Save_Handler(Transport transport)
        {
            _view.SaveIndex = _model.Save(transport);
        }

        private void Update_Handler(Transport transport)
        {
            _model.Update(transport);
        }

        private void Delete_Handler(List<Transport> transports)
        {
            _model.Delete(transports);
        }

        private void GetAll_Handler()
        {
            _view.Transports = _model.GetAll();
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
