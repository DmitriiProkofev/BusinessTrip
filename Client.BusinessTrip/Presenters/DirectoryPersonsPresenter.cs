using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представитель "Справочник сотрудников".
    /// </summary>
    public class DirectoryPersonsPresenter : IDirectoryPersonsPresenter
    {
        #region Private Fields

        private IDirectoryPersonsView _view;
        private IBaseModel<Person> _model;

        #endregion //Private Fields

        #region IDirectoryPersonsPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryPersonsView view, IBaseModel<Person> model)
        {
            _view = view;
            _model = model;

            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;
        }

        #endregion //IDirectoryPersonsPresenter

        #region Event Handlers

        private void Save_Handler(Person person)
        {
            _view.SaveIndex = _model.Save(person);
        }

        private void Update_Handler(Person person)
        {
            _model.Update(person);
        }

        private void Delete_Handler(List<Person> persons)
        {
            _model.Delete(persons);
        }

        private void GetAll_Handler()
        {
            _view.Persons = _model.GetAll();
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
