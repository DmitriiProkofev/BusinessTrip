using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник отделов". 
    /// </summary>
    public interface IDirectoryDepartmentsView : IBaseView<Department>
    {
        /// <summary>
        /// Отделы.
        /// </summary>
        List<Department> Departments { get; set; }

        /// <summary>
        /// Текущая выбранный отдел.
        /// </summary>
        Department CurrentDepartment { get; set; }
    }
}
