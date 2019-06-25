using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessTrip.Helpers.DomainHelpers
{
    public class BoolComparer : IComparer<bool>
    {
        public int Compare(bool x, bool y)
        {
            int p = x ? 0 : 1;
            int q = y ? 0 : 1;
            return p - q;
        }
    }
}
