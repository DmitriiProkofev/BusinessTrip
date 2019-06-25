using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник должностей".
    /// </summary>
    public class DirectoryPositionsPresenter : IDirectoryPositionsPresenter
    {
        #region Private Fields

        private IDirectoryPositionsView _view;
        private IBaseModel<Position> _model;

        #endregion //Private Fields

        #region IDirectoryPositionPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryPositionsView view, IBaseModel<Position> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryPositionPresenter

        #region Event Handlers

        private void Save_Handler(Position position)
        {
            _view.SaveIndex = _model.Save(position);
        }

        private void Update_Handler(Position position)
        {
            _model.Update(position);
        }

        private void Delete_Handler(List<Position> positions)
        {
            _model.Delete(positions);
        }

        private void GetAll_Handler()
        {
            _view.Positions = _model.GetAll();
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
