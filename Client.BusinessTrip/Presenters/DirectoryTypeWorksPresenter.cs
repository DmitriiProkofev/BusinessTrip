using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представитель "Справочник видов работ".
    /// </summary>
    public class DirectoryTypeWorksPresenter : IDirectoryTypeWorksPresenter
    {
        #region Private Fields

        private IDirectoryTypeWorksView _view;
        private IBaseModel<TypeWork> _model;

        #endregion //Private Fields

        #region IDirectoryTypeWorksPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryTypeWorksView view, IBaseModel<TypeWork> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryTypeWorksPresenter

        #region Event Handlers

        private void Save_Handler(TypeWork typeWork)
        {
            _view.SaveIndex = _model.Save(typeWork);
        }

        private void Update_Handler(TypeWork typeWork)
        {
            _model.Update(typeWork);
        }

        private void Delete_Handler(List<TypeWork> typeWorks)
        {
            _model.Delete(typeWorks);
        }

        private void GetAll_Handler()
        {
            _view.TypeWorks = _model.GetAll();
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
