using EmployeDataModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace rdbm
{
    public class ProjectGateWay
    {
        private readonly MDFConnection con;

        public ProjectGateWay(MDFConnection connection)
        {
            this.con = connection;
        }

        public IEnumerable<Project> GetAll()
        {

        }
    }
}