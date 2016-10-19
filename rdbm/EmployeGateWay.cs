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
                BSN = reader.GetInt32(0),
                Name = reader.GetString(1),
                SurName = reader.GetString(2),
                BuildingName = reader.GetString(3)
                
            };
        }

        public IEnumerable<Employee> GetAll()
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            using (var cmd = new SqlCommand("Select * From [Employee]", con.connection))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return map(reader);
                }
            }
        }

        public Employee FindByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            using (var cmd = new SqlCommand("Select TOP 1 * From [Employee] WHERE [Employee].[BSN] = @BSN", con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return map(reader);
                }
            }

        }

        public void addHQ(HeadQuater hq)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            using (var cmd = new SqlCommand(string.Format("INSERT INTO [HeadQuater] values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                hq.BuildingName, hq.Rooms, hq.Rent, hq.Country, hq.PostCode, hq.HouseNumber), con.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void addADR(Address adr)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            using (var cmd = new SqlCommand(string.Format("INSERT INTO [Address] values ('{0}', '{1}', '{2}', '{3}', '{4}')",
                adr.Country, adr.PostCode, adr.HouseNumber, adr.City, adr.Street), con.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Add(Employee employee)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            using (var cmd = new SqlCommand(string.Format("INSERT INTO [Employe] values ('{0}', '{1}', '{2}', '{3}')", employee.BSN, employee.Name, employee.SurName, employee.BuildingName), con.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
              
        
    }
}
