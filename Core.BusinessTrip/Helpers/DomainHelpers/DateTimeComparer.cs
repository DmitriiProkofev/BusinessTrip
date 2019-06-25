using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessTrip.Helpers.DomainHelpers
{
    public class DateTimeComparer : IComparer<DateTime?>
    {
        #region IComparer<DateTime?> Members

        public int Compare(DateTime? x, DateTime? y)
        {
            DateTime nx = x ?? DateTime.MaxValue;
            DateTime ny = y ?? DateTime.MaxValue;

            return nx.CompareTo(ny);
        }

        #endregion
    }
}
