using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public interface IContext
    {
        EmployeeGateWay Employees { get; }
        HeadQuaterGateWay HeadQuaters { get; }
    }


    public class Context : IContext
    {
        public EmployeeGateWay Employees { get; }
        public HeadQuaterGateWay HeadQuaters { get; }

        public Context()
        {
            var connection = new MDFConnection();

            Employees = new EmployeeGateWay(connection, this);
            HeadQuaters = new HeadQuaterGateWay(connection);
        }
    }


}
