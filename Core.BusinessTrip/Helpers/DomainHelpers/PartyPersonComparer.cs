using Core.BusinessTrip.Domain;
using System.Collections.Generic;

namespace Core.BusinessTrip.Helpers.DomainHelpers
{
    public class PartyPersonComparer : IComparer<PartyPerson>
    {
        #region IComparer Members

        public int Compare(PartyPerson x, PartyPerson y)
        {
            BoolComparer boolComparer = new BoolComparer();
            var oneSort = boolComparer.Compare(x.IsResponsible, y.IsResponsible);

            if (oneSort != 0)
                return oneSort;

            return string.Compare(x.Name, y.Name);
        }

        #endregion
    }
}
