using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Командировка".
    /// </summary>
    public interface IBusinessTripView : IBaseView<Core.BusinessTrip.Domain.BusinessTrip>
    {
        /// <summary>
        /// Список командировок.
        /// </summary>
        List<Core.BusinessTrip.Domain.BusinessTrip> BusinessTrips { get;  set; }

        /// <summary>
        /// Текущая выбранная командировка.
        /// </summary>
        Core.BusinessTrip.Domain.BusinessTrip CurrentBusinessTrip { get; set; }

        /// <summary>
        /// Ид руководителя структурного подразделения.
        /// </summary>
        int? HeadStructuralDivisionId { get; set; }

        /// <summary>
        /// Ид главы организации.
        /// </summary>
        int? HeadOrganizationId { get; set; }

        /// <summary>
        /// Ид автора отчета.
        /// </summary>
        int? AuthoredId { get; set; }

        /// <summary>
        /// Ид вида работ.
        /// </summary>
        int? TypeWorkId { get; set; }

        /// <summary>
        /// Ид партии.
        /// </summary>
        int? PartyId { get; set; }

        /// <summary>
        /// Ид направлений.
        /// </summary>
        List<Direction> Directions { get; set; }

        List<PartyPerson> PartyPersons { get; set; }

        Dictionary<string, string> TargetReasons { get; set; }

        /// <summary>
        /// Событие запускает получение направлений по ид командировки.
        /// </summary>
        event Action<int> GetDirectionsByBusinessTripId;

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        event Action<Core.BusinessTrip.Domain.BusinessTrip> SaveBusinessTrip;

        /// <summary>
        /// Событие запускает удаление направление.
        /// </summary>
        event Action<List<int>> DeleteDirectionByIds;

        event Action<List<int>> ExportToDoc;

        event Action<List<string>> GetTargetsByReasons;

        event Action<int> GetPartyPersonsByPartyId;

        /// <summary>
        /// Ид ответственного за оборудование.
        /// </summary>
        int? EquipmentId { get; set; }

        /// <summary>
        /// Ид ответственного за данные. 
        /// </summary>
        int? DataId { get; set; }

        /// <summary>
        /// Ид ответственного за связь.
        /// </summary>
        int? CommunicationId { get; set; }

        /// <summary>
        /// Ид ответственного за техническую безопасность.
        /// </summary>
        int? TechnicalSecurityId { get; set; }

        /// <summary>
        /// Ид ответственного за пожарную безопасность.
        /// </summary>
        int? FireSecurityId { get; set; }

        /// <summary>
        /// Ид ответственного за промышленную безопасность.
        /// </summary>
        int? IndustrialSecurityId { get; set; }

        /// <summary>
        /// Ид ответственного за осмотр буровой.
        /// </summary>
        int? RigInspectionId { get; set; }

        /// <summary>
        /// Ид ответственного за сдачу материала.
        /// </summary>
        int? DealerId { get; set; }

        /// <summary>
        /// Ид ответственного за прием материала.
        /// </summary>
        int? ReceivingId { get; set; }

        /// <summary>
        /// Ид ответственного за контроль выполнения приказа.
        /// </summary>
        int? MonitoringId { get; set; }

        /// <summary>
        /// Ид ответственного за оповещение.
        /// </summary>
        int? InformationId { get; set; }

        /// <summary>
        /// Ид сотрудника отдела управления проектами.
        /// </summary>
        int? ProjectManagerId { get; set; }

        /// <summary>
        /// Ид сотрудника заказывающего транспорт.
        /// </summary>
        int? TransportCustomerId { get; set; }

        /// <summary>
        /// Ид транспорта.
        /// </summary>
        int? MarkId { get; set; }

        /// <summary>
        /// Ид водителя.
        /// </summary>
        int? DriverNameId { get; set; }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        int? AddressId { get; set; }
    }
}
