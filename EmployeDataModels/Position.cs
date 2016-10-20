using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeDataModels
{
    public class Position
    {
        public string PositionName { get; set; }
        public string Description { get; set; }
        public float HourFee { get; set; }

        public int BSN { get; set; }

        public Employee Employee { get; set; }
    }
}
