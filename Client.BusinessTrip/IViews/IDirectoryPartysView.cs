using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;

namespace Client.BusinessTrip.IViews
{
    /// <summary>
    /// Интерфейс представления "Справочник партий".
    /// </summary>
    public interface IDirectoryPartysView : IBaseView<Party>
    {
        /// <summary>
        /// Список партий.
        /// </summary>
        List<Party> Partys { get; set; }

        /// <summary>
        /// Текущая выбранная партия.
        /// </summary>
        Party CurrentParty { get; set; }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        List<PartyPerson> PartyPersons { get; set; }

        /// <summary>
        /// Сотрудники для добавления.
        /// </summary>
        List<PartyPerson> AddPersons { get; set; }

        /// <summary>
        /// Сотрудники на удаление.
        /// </summary>
        List<PartyPerson> DeletePersons { get; set; }

        /// <summary>
        /// Сотрудники для редактирования.
        /// </summary>
        List<PartyPerson> EditPersons { get; set; }

        /// <summary>
        /// Сохранение партии.
        /// </summary>
        event Action<Party, List<PartyPerson>> SaveParty;

        /// <summary>
        /// Обновление партии.
        /// </summary>
        event Action<Party, List<PartyPerson>, List<PartyPerson>, List<PartyPerson>> UpdateParty;
    }
}
