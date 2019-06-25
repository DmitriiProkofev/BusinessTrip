using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник адресов".
    /// </summary>
    public class DirectoryLocationsPresenter : IDirectoryLocationsPresenter
    {
        #region Private Fields

        private IDirectoryLocationsView _view;
        private IBaseModel<Location> _model;

        #endregion //Private Fields

        #region IDirectoryLocationsPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryLocationsView view, IBaseModel<Location> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryLocationsPresenter

        #region Event Handlers

        private void Save_Handler(Location location)
        {
            _view.SaveIndex = _model.Save(location);
        }

        private void Update_Handler(Location location)
        {
            _model.Update(location);
        }

        private void Delete_Handler(List<Location> locations)
        {
            _model.Delete(locations);
        }

        private void GetAll_Handler()
        {
            _view.Locations = _model.GetAll();
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
