using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник сотрудников".
    /// </summary>
    public interface IDirectoryPersonsView : IBaseView<Person>
    {
        /// <summary>
        /// Список сотрудников.
        /// </summary>
        List<Person> Persons { get; set; }

        /// <summary>
        /// Текущий выбранный сотрудник.
        /// </summary>
        Person CurrentPerson { get; set; }

        /// <summary>
        /// Ид должности.
        /// </summary>
        int? PositionId { get; set; }

        /// <summary>
        /// Ид отдела.
        /// </summary>
        int? DepartmentId { get; set; }

        /// <summary>
        /// Ид руководителя.
        /// </summary>
        int? HeadId { get; set; }

        /// <summary>
        /// Ид адреса.
        /// </summary>
        int? LocationId { get; set; }
    }
}
