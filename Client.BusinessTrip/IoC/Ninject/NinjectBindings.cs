using Client.BusinessTrip.IViews;
using Client.BusinessTrip.Views;
using Ninject.Modules;
using Core.BusinessTrip.Domain;
using Client.BusinessTrip.IModels;
using Client.BusinessTrip.Models;
using Client.BusinessTrip.IPresenters;
using Client.BusinessTrip.Presenters;
using Core.BusinessTrip.DataInterfaces;
using Data.BusinessTrip.Repository;

namespace Client.BusinessTrip.IoC.Ninject
{
    /// <summary>
    /// Класс внедрения зависимостей.
    /// </summary>
    public class NinjectBindings : NinjectModule
    {
        /// <summary>
        /// Внедрение зависимостей.
        /// </summary>
        public override void Load()
        {
            #region Repository

            Bind<IExtendedRepositoryBusinessTrip, IRepository<Core.BusinessTrip.Domain.BusinessTrip>>().To<ExtendedRepositoryBusinessTrip>();
            Bind<IRepository<Command>>().To<Repository<Command>>();
            Bind<IRepository<Direction>>().To<Repository<Direction>>();
            Bind<IRepository<Location>>().To<Repository<Location>>();
            Bind<IRepository<Organization>>().To<Repository<Organization>>();
            Bind<IExtendedRepositoryParty, IRepository<Party>>().To<ExtendedRepositoryParty>();
            Bind<IRepository<Person>>().To<Repository<Person>>();
            Bind<IRepository<RequestTransport>>().To<Repository<RequestTransport>>();
            Bind<IRepository<TypeWork>>().To<Repository<TypeWork>>();
            Bind<IRepository<Position>>().To<Repository<Position>>();
            Bind<IRepository<Department>>().To<Repository<Department>>();
            Bind<IRepository<Transport>>().To<Repository<Transport>>();

            #endregion //Repository

            #region MVP

            //BusinessTrip
            Bind<IBusinessTripView, IBaseView<Core.BusinessTrip.Domain.BusinessTrip>>().To<BusinessTripView>();
            Bind<IBusinessTripPresenter>().To<BusinessTripPresenter>();
            Bind<IBusinessTripModel, IBaseModel<Core.BusinessTrip.Domain.BusinessTrip>>().To<BusinessTripModel>();

            //DirectoryPersons
            Bind<IDirectoryPersonsView, IBaseView<Person>>().To<DirectoryPersonsView>();
            Bind<IDirectoryPersonsPresenter>().To<DirectoryPersonsPresenter>();
            Bind<IBaseModel<Person>>().To<DirectoryPersonsModel>();

            //DirectoryLocations
            Bind<IDirectoryLocationsView, IBaseView<Location>>().To<DirectoryLocationsView>();
            Bind<IDirectoryLocationsPresenter>().To<DirectoryLocationsPresenter>();
            Bind<IBaseModel<Location>>().To<DirectoryLocationsModel>();

            //DirectoryOrganizations
            Bind<IDirectoryOrganizationsView, IBaseView<Organization>>().To<DirectoryOrganizationsView>();
            Bind<IDirectoryOrganizationsPresenter>().To<DirectoryOrganizationsPresenter>();
            Bind<IBaseModel<Organization>>().To<DirectoryOrganizationsModel>();

            //DirectoryDirections
            Bind<IDirectoryDirectionsView, IBaseView<Direction>>().To<DirectoryDirectionsView>();
            Bind<IDirectoryDirectionsPresenter>().To<DirectoryDirectionsPresenter>();
            Bind<IBaseModel<Direction>>().To<DirectoryDirectionsModel>();

            //DirectoryTypeWorks
            Bind<IDirectoryTypeWorksView, IBaseView<TypeWork>>().To<DirectoryTypeWorksView>();
            Bind<IDirectoryTypeWorksPresenter>().To<DirectoryTypeWorksPresenter>();
            Bind<IBaseModel<TypeWork>>().To<DirectoryTypeWorksModel>();

            //DirectoryPartys
            Bind<IDirectoryPartysView, IBaseView<Party>>().To<DirectoryPartysView>();
            Bind<IDirectoryPartysPresenter>().To<DirectoryPartysPresenter>();
            Bind<IDirectoryPartysModel, IBaseModel<Party>>().To<DirectoryPartysModel>();

            //DirectoryPositions
            Bind<IDirectoryPositionsView, IBaseView<Position>>().To<DirectoryPositionsView>();
            Bind<IDirectoryPositionsPresenter>().To<DirectoryPositionsPresenter>();
            Bind<IBaseModel<Position>>().To<DirectoryPositionsModel>();

            //DirectoryDepartments
            Bind<IDirectoryDepartmentsView, IBaseView<Department>>().To<DirectoryDepartmentsView>();
            Bind<IDirectoryDepartmentsPresenter>().To<DirectoryDepartmentsPresenter>();
            Bind<IBaseModel<Department>>().To<DirectoryDepartmentsModel>();

            //DirectoryTransports
            Bind<IDirectoryTransportsView, IBaseView<Transport>>().To<DirectoryTransportsView>();
            Bind<IDirectoryTransportsPresenter>().To<DirectoryTransportsPresenter>();
            Bind<IBaseModel<Transport>>().To<DirectoryTransportsModel>();

            #endregion //MVP
        }
    }
}
