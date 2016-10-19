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

        /// <summary>
        /// get all users excluding his/her Adresses
        /// </summary>
        /// <returns></returns>
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

        private Employee fullMap(SqlDataReader reader)
        {
            var emp = new Employee
            {
                BSN = reader.GetInt32(0),
                Name = reader.GetString(1),
                SurName = reader.GetString(2),
                BuildingName = reader.GetString(3),
                Adresses = new List<EmployeeAddress>()

            };
            do
            {
                //there is no result
                if(!reader.IsDBNull(4))
                    ((List<EmployeeAddress>)emp.Adresses).Add(new EmployeeAddress
                    {
                        BSN = emp.BSN,
                        Residence = reader.GetBoolean(4),
                        Country = reader.GetString(5),
                        PostCode = reader.GetString(6),
                        HouseNumber = reader.GetString(7),
                        Employee = emp,
                        Address = new Address
                        {
                            Country = reader.GetString(5),
                            PostCode = reader.GetString(6),
                            HouseNumber = reader.GetString(7),
                            City = reader.GetString(8),
                            Street = reader.GetString(9)
                        }
                    });
            }
            while (reader.Read());

            return emp;
        }

        public Employee FindByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql =
                    @"Select [Employee].*, [EmployeeAddress].Residence, [Address].*
                    From [Employee]
                    LEFT JOIN [EmployeeAddress] ON [Employee].[BSN] = [EmployeeAddress].[BSN]
                    LEFT JOIN [Address] ON
	                    [EmployeeAddress].[Country] = [Address].[Country] AND 
	                    [EmployeeAddress].[PostCode] = [Address].[PostCode] AND 
	                    [EmployeeAddress].[HouseNumber] = [Address].[HouseNumber]
                    WHERE 
	                    [Employee].[BSN] = @BSN";

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return fullMap(reader);
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
