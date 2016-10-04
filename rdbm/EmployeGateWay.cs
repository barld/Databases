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

        public void add(Employee employee)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            using (var cmd = new SqlCommand(string.Format("INSERT INTO [Employe] values ('{0}', '{1}', '{2}', '{3}')", employee.BSN, employee.Name, employee.SurName, employee.BuildingName), con.connection))
            {
                cmd.ExecuteNonQuery();
            }
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
