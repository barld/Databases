using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeDataModels
{
    public class Employee
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int BSN { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// fk to HeadQuater
        /// </summary>
        public string BuildingName { get; set; }

        public HeadQuater HeadQuater { get; set; }
    }
}
