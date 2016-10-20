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
        DegreesGateWay Degrees { get; }
        PositionGateWay Positions { get; }
    }


    public class Context : IContext
    {
        public EmployeeGateWay Employees { get; }
        public HeadQuaterGateWay HeadQuaters { get; }
        public DegreesGateWay Degrees { get; }
        public PositionGateWay Positions { get; }

        public Context()
        {
            var connection = new MDFConnection();

            Employees = new EmployeeGateWay(connection, this);
            HeadQuaters = new HeadQuaterGateWay(connection);
            Degrees = new DegreesGateWay(connection);
            Positions = new PositionGateWay(connection);
        }
    }


}
