using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник направлений".
    /// </summary>
    public class DirectoryDirectionsPresenter : IDirectoryDirectionsPresenter
    {
        #region Private Fields

        private IDirectoryDirectionsView _view;
        private IBaseModel<Direction> _model;

        #endregion //Private Fields

        #region IDirectoryDirectionsPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryDirectionsView view, IBaseModel<Direction> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryDirectionsPresenter

        #region Event Handlers

        private void Save_Handler(Direction direction)
        {
            _model.Save(direction);
        }

        private void Update_Handler(Direction direction)
        {
            _model.Update(direction);
        }

        private void Delete_Handler(List<Direction> directions)
        {
            _model.Delete(directions);
        }

        private void GetAll_Handler()
        {
            //_view.Directions = _model.GetAll();
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
