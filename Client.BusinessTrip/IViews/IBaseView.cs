using System;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Базовый интерфейс представления объектов.
    /// </summary>
    /// <typeparam name="T">Объект.</typeparam>
    public interface IBaseView<T> where T : class
    {
        /// <summary>
        /// Индекс выделенной строки.
        /// </summary>
        int? SaveIndex { get; set; }

        /// <summary>
        /// Событие запускает сохранение объекта.
        /// </summary>
        event Action<T> Save;

        /// <summary>
        /// Событие запускает обновление объекта.
        /// </summary>
        event Action<T> Update;

        /// <summary>
        /// Событие запускает удаление объектов.
        /// </summary>
        event Action<List<T>> Delete;

        /// <summary>
        /// Событие запускает получение объектов.
        /// </summary>
        event Action GetAll;

        /// <summary>
        /// Событие закрытия приложения.
        /// </summary>
        event Action ViewClosed;
    }
}
