using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeDataModels
{
    public class Degree
    {
        public string Course { get; set; }
        public string School { get; set; }
        public string Level { get; set; }
        /// <summary>
        /// fk: Employe
        /// </summary>
        public int BSN { get; set; }


        public Employee Employee { get; set; }
    }
}
