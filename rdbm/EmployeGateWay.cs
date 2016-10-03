using EmployeDataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public class EmployeeGateWay
    {
        private readonly MDFConnection con;

        public EmployeeGateWay(MDFConnection connection)
        {
            con = connection;
        }

        private Employee map(SqlDataReader reader)
        {
            return new Employee
            {
                BSN = reader.GetInt32(0)
                //..
            };
        }

        public IEnumerable<Employee> GetAll()
        {
            if(con.connection.State != ConnectionState.Open)
                con.connection.Open();
            using (var cmd = new SqlCommand("Select * From [Employe]", con.connection))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return map(reader);
                }
            }
        }
    }
}
