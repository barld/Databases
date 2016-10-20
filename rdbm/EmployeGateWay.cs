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
        private readonly IContext context;

        internal EmployeeGateWay(MDFConnection connection, IContext context)
        {
            con = connection;
            this.context = context;
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

        public void Update(Employee employee)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            //update employee record
            var sql = @"UPDATE [Employee]
                SET Name = @name, SurName = @surName, BuildingName = @bname
                WHERE BSN = @BSN";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = employee.Name;

                cmd.Parameters.Add("@surName", SqlDbType.VarChar);
                cmd.Parameters["@surName"].Value = employee.SurName;

                cmd.Parameters.Add("@bname", SqlDbType.VarChar);
                cmd.Parameters["@bname"].Value = employee.BuildingName;

                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = employee.BSN;
            }

            //dirty way of updating
            var sqlDEA = "DELETE FROM [EmployeeAddress] WHERE BSN = @BSN;";
            using (var cmd = new SqlCommand(sqlDEA, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = employee.BSN;

                cmd.ExecuteNonQuery();
            }

            CreateEmployeeAdresses(employee);
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
            // TODO: add logic to check if addresse exists and add conection to the employee
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"INSERT INTO [Employee](BSN, Name, SurName, BuildingName) 
                values (@BSN, @Name, @Surname, @BuildingName)";

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = employee.BSN;

                cmd.Parameters.Add("@Name", SqlDbType.VarChar);
                cmd.Parameters["@Name"].Value = employee.Name;

                cmd.Parameters.Add("@Surname", SqlDbType.VarChar);
                cmd.Parameters["@Surname"].Value = employee.SurName;

                cmd.Parameters.Add("@BuildingName", SqlDbType.VarChar);
                cmd.Parameters["@BuildingName"].Value = employee.BuildingName;


                cmd.ExecuteNonQuery();
            }

            CreateEmployeeAdresses(employee);

        }

        private void CreateEmployeeAdresses(Employee employee)
        {
            var rdbmHeadQuaterGateWay = context.HeadQuaters as HeadQuaterGateWay;
            foreach (var ea in employee.Adresses)
            {
                rdbmHeadQuaterGateWay.AddIfNotExists(ea.Address);
                //add adress for employee
                var sqlEA = @"INSERT INTO [EmployeeAddress](BSN, Country, PostCode, HouseNumber, Residence)
                    VALUES (@BSN, @country, @postCode, @houseNumber, @residence)";
                using (var cmd = new SqlCommand(sqlEA, con.connection))
                {
                    cmd.Parameters.Add("@BSN", SqlDbType.Int);
                    cmd.Parameters["@BSN"].Value = employee.BSN;

                    cmd.Parameters.Add("@country", SqlDbType.VarChar);
                    cmd.Parameters["@country"].Value = ea.Country;

                    cmd.Parameters.Add("@postCode", SqlDbType.VarChar);
                    cmd.Parameters["@postCode"].Value = ea.PostCode;

                    cmd.Parameters.Add("@houseNumber", SqlDbType.VarChar);
                    cmd.Parameters["@houseNumber"].Value = ea.HouseNumber;

                    cmd.Parameters.Add("@residence", SqlDbType.Bit);
                    cmd.Parameters["@residence"].Value = ea.Residence;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"DELETE FROM [Employee] WHERE [Employee].[BSN] = @BSN";

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;


                cmd.ExecuteNonQuery();
            }
        }
              
        
    }
}
