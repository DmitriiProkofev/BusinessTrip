using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представитель "Справочник партий".
    /// </summary>
    public class DirectoryPartysPresenter : IDirectoryPartysPresenter
    {
        #region Private Fields

        private IDirectoryPartysView _view;
        private IDirectoryPartysModel _model;

        #endregion //Private Fields

        #region IDirectoryPartysPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IDirectoryPartysView view, IDirectoryPartysModel model)
        {
            _view = view;
            _model = model;

            if (_view.PartyPersons == null)
                _view.PartyPersons = new List<PartyPerson>();

            if (_view.Partys == null)
                _view.Partys = new List<Party>();

            _view.SaveParty += SaveParty_Handler;
            _view.UpdateParty += UpdateParty_Handler;
            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;

        }

        #endregion //IDirectoryPartysPresenter

        #region Event Handlers

        private void SaveParty_Handler(Party party, List<PartyPerson> persons)
        {
            _view.SaveIndex = _model.SaveParty(party, persons);
        }

        private void UpdateParty_Handler(Party party, List<PartyPerson> addPersons, List<PartyPerson> deletePersons, List<PartyPerson> editPersons)
        {
            _model.UpdateParty(party, addPersons, deletePersons, editPersons);
        }

        private void Save_Handler(Party party)
        {
            _model.Save(party);
        }

        private void Update_Handler(Party party)
        {
            _model.Update(party);
        }

        private void Delete_Handler(List<Party> partys)
        {
            _model.Delete(partys);
        }

        private void GetAll_Handler()
        {
            _view.Partys = _model.GetAll();
        }

        private void ViewClosed_Handler()
        {
            _view.SaveParty -= SaveParty_Handler;
            _view.UpdateParty -= UpdateParty_Handler;
            _view.Save -= Save_Handler;
            _view.Update -= Update_Handler;
            _view.Delete -= Delete_Handler;
            _view.GetAll -= GetAll_Handler;
            _view.ViewClosed -= ViewClosed_Handler;
        }

        #endregion //Event Handlers
    }
}
