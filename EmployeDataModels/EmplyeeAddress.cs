using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeDataModels
{
    public class EmployeeAddress
    {
        public int BSN { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string HouseNumber { get; set; }
        public bool Residence { get; set; }

        public Employee Employee { get; set; }
        public Address Address { get; set; }
    }
}
