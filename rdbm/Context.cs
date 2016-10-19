using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public interface IContext
    {
        EmployeeGateWay Employee { get; }
    }


    public class Context : IContext
    {
        public EmployeeGateWay Employee { get; }

        public Context()
        {
            var connection = new MDFConnection();

            Employee = new EmployeeGateWay(connection);
        }
    }


}
