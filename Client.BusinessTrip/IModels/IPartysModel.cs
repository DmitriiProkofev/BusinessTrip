using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IModels
{
    /// <summary>
    /// Интерфейс модели "Справочник партий".
    /// </summary>
    public interface IDirectoryPartysModel : IBaseModel<Party>
    {
        /// <summary>
        /// Сохранение партий.
        /// </summary>
        /// <param name="party">Партия.</param>
        /// <param name="persons">Список сотрудников.</param>
        int? SaveParty(Party party, List<PartyPerson> persons);

        /// <summary>
        /// Обновление партии.
        /// </summary>
        /// <param name="party">Партия.</param>
        /// <param name="addPersons">Новые сотрудники в партии.</param>
        /// <param name="deletePersons">Сотрудники в партии к удалению.</param>
        /// <param name="editPersons">Сотрудники в партии для редактирования.</param>
        void UpdateParty(Party party, List<PartyPerson> addPersons, List<PartyPerson> deletePersons, List<PartyPerson> editPersons);
    }
}
