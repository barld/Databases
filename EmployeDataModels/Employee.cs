using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int BSN { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        /// <summary>
        /// fk to HeadQuater
        /// </summary>
        public string BuildingName { get; set; }

        public HeadQuater HeadQuater { get; set; }

        public virtual IEnumerable<Degree> Degrees { get; set; }
        public virtual IEnumerable<EmployeeAddress> Adresses { get; set; }
        public virtual IEnumerable<Position> Positions { get; set; }
    }
}
