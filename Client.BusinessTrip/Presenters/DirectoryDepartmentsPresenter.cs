using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Справочник отделов".
    /// </summary>
    public class DirectoryDepartmentsPresenter : IDirectoryDepartmentsPresenter
    {
        #region Private Fields

        private IDirectoryDepartmentsView _view;
        private IBaseModel<Department> _model;

        #endregion //Private Fields

        #region IDirectoryDepartmentPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryDepartmentsView view, IBaseModel<Department> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryDepartmentPresenter

        #region Event Handlers

        private void Save_Handler(Department department)
        {
            _view.SaveIndex = _model.Save(department);
        }

        private void Update_Handler(Department department)
        {
            _model.Update(department);
        }

        private void Delete_Handler(List<Department> departments)
        {
            _model.Delete(departments);
        }

        private void GetAll_Handler()
        {
            _view.Departments = _model.GetAll();
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
