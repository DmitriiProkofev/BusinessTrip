using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessTrip.Domain
{
    public class ContractPIR
    {
        public string Code { get; set; }


        private string _target;
        public string Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value!=null)
                {
                    _target = value;
                }
            }
        }
    }
}
