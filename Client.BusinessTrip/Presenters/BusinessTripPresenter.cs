using Client.BusinessTrip.IModels;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.IViews;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.Presenters
{
    /// <summary>
    /// Класс представителя "Командировка".
    /// </summary>
    public class BusinessTripPresenter : IBusinessTripPresenter
    {
        #region Private Fields

        private IBusinessTripView _view;
        private IBusinessTripModel _model;

        #endregion //Private Fields

        #region IBusinessTripPresenter

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="model">Модель.</param>
        public void Init(IBusinessTripView view, IBusinessTripModel model)
        {
            _view = view;
            _model = model;

            if (_view.Directions == null)
                _view.Directions = new List<Direction>();

            if (_view.PartyPersons == null)
                _view.PartyPersons = new List<PartyPerson>();

            if (_view.BusinessTrips == null)
                _view.BusinessTrips = new List<Core.BusinessTrip.Domain.BusinessTrip>();

            //if (_view.TypeWorks == null)
            //    _view.TypeWorks = new List<TypeWork>();

            _view.DeleteDirectionByIds += DeleteDirectionByIds_Handler;
            _view.SaveBusinessTrip += SaveBusinessTrip_Handler;
            _view.GetDirectionsByBusinessTripId += GetDirectionsByBusinessTripId_Handler;
            _view.Save += Save_Handler;
            _view.Update += Update_Handler;
            _view.Delete += Delete_Handler;
            _view.GetAll += GetAll_Handler;
            _view.ViewClosed += ViewClosed_Handler;
            _view.GetTargetsByReasons += GetTargetsByReasons_Handler;
            _view.GetPartyPersonsByPartyId += GetPartyPersonsByPartyId_Handler;

        }

        #endregion //IBusinessTripPresenter

        #region Event Handlers

        private void DeleteDirectionByIds_Handler(List<int> directionids)
        {
            _model.DeleteDirectionByIds(directionids);
        }
        private void SaveBusinessTrip_Handler(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            _view.SaveIndex = _model.SaveBusinessTrip(businessTrip);
        }

        private void GetDirectionsByBusinessTripId_Handler(int businessTripid)
        {
            _view.Directions = _model.GetDirectionsByBusinessTripId(businessTripid); 
        }

        private void Save_Handler(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            _view.SaveIndex = _model.Save(businessTrip);
        }

        private void Update_Handler(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            _model.Update(businessTrip);
        }

        private void Delete_Handler(List<Core.BusinessTrip.Domain.BusinessTrip> businessTrips)
        {
            _model.Delete(businessTrips);
        }

        private void GetAll_Handler()
        {
            _view.BusinessTrips = _model.GetAll();
        }

        private void GetTargetsByReasons_Handler(List<string> reasons)
        {
            _view.TargetReasons = _model.GetTargetsByReasons(reasons);
        }

        private void GetPartyPersonsByPartyId_Handler(int partyId)
        {
            _view.PartyPersons = _model.GetPartyPersonsByPartyId(partyId);
        }

        private void ViewClosed_Handler()
        {
            _view.DeleteDirectionByIds -= DeleteDirectionByIds_Handler;
            _view.SaveBusinessTrip -= SaveBusinessTrip_Handler;
            _view.GetDirectionsByBusinessTripId -= GetDirectionsByBusinessTripId_Handler;
            _view.Save -= Save_Handler;
            _view.Update -= Update_Handler;
            _view.Delete -= Delete_Handler;
            _view.GetAll -= GetAll_Handler;
            _view.ViewClosed -= ViewClosed_Handler;
            _view.GetTargetsByReasons -= GetTargetsByReasons_Handler;
            _view.GetPartyPersonsByPartyId -= GetPartyPersonsByPartyId_Handler;
        }

        #endregion //Event Handlers
    }
}
