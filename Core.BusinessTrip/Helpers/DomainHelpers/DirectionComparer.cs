using Core.BusinessTrip.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.BusinessTrip.Helpers.DomainHelpers
{
    public class DirectionComparer : IComparer<Direction>
    {
        //public enum ComparisonType
        //{
        //    DateBegin = 1,
        //    DateEnd = 2
        //}

        //private ComparisonType _comparisonType;


        //public ComparisonType ComparisonMethod
        //{
        //    get { return _comparisonType; }
        //    set { _comparisonType = value; }
        //}

        #region IComparer Members

        public int Compare(Direction x, Direction y)
        {
            //ComparisonMethod = ComparisonType.DateBegin;

            DateTimeComparer dateTimeComparer = new DateTimeComparer();

            var oneSort = dateTimeComparer.Compare(x.DateBegin.Value.Date, y.DateBegin.Value.Date);

            if (oneSort != 0)
                return oneSort;

            return dateTimeComparer.Compare(x.DateEnd.Value.Date, y.DateEnd.Value.Date);


            //var oneSort = x.CompareTo(y, _comparisonType);
            //if (oneSort != 0)
            //    return oneSort;

            //ComparisonMethod = ComparisonType.DateEnd;
            //return x.CompareTo(y, _comparisonType); 
        }

        #endregion




        //private int _sortOrder = 1;

        //public DirectionComparer(SortOrder sortOrder)
        //{
        //    _sortOrder = sortOrder == SortOrder.Ascending ? 1 : -1;
        //}

        //public int Compare(Direction x, Direction y)
        //{
        //    DataGridViewRow row1 = (DataGridViewRow)x;
        //    DataGridViewRow row2 = (DataGridViewRow)y;
        //}
    }
}
