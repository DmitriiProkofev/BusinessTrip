using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Client.BusinessTrip.IModels
{
    /// <summary>
    /// Интерфейс модели "Командировки".
    /// </summary>
    public interface IBusinessTripModel : IBaseModel<Core.BusinessTrip.Domain.BusinessTrip>
    {
        /// <summary>
        /// Получение направлений по Ид командировки.
        /// </summary>
        /// <param name="businessTripId">Ид командировки.</param>
        /// <returns>Список направлений.</returns>
        List<Direction> GetDirectionsByBusinessTripId(int businessTripId);

        /// <summary>
        /// Сохранение командировки.
        /// </summary>
        /// <param name="businessTrip">Командировка.</param>
        int? SaveBusinessTrip(Core.BusinessTrip.Domain.BusinessTrip businessTrip);

        /// <summary>
        /// Удаление направлений по Ид.
        /// </summary>
        /// <param name="directionIds">Ид направлений.</param>
        void DeleteDirectionByIds(List<int> directionIds);

        Dictionary<string, string> GetTargetsByReasons(List<string> reasons);

        List<Core.BusinessTrip.Domain.BusinessTrip> GetBusinessTripFullByIds(List<int> businessTripIds);

        List<PartyPerson> GetPartyPersonsByPartyId(int partyId);
    }
}
