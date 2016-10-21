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
    public class AddressGateWay
    {
        private readonly MDFConnection con;

        public AddressGateWay(MDFConnection connection)
        {
            con = connection;
        }

        public bool Exists(Address address) =>
            Exists(address.Country, address.PostCode, address.HouseNumber);

        public bool Exists(string country, string postCode, string houseNumber)
        {
            var sql = @"SELECT COUNT(*) FROM [Address] 
            WHERE [Address].[Country] = @country AND [Address].[PostCode] = @postCode
            AND [Address].[HouseNumber] = @houseNumber;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@country", SqlDbType.VarChar);
                cmd.Parameters["@country"].Value = country;

                cmd.Parameters.Add("@postCode", SqlDbType.VarChar);
                cmd.Parameters["@postCode"].Value = postCode;

                cmd.Parameters.Add("@houseNumber", SqlDbType.VarChar);
                cmd.Parameters["@houseNumber"].Value = houseNumber;

                return 1 == (int)cmd.ExecuteScalar();
            }
        }

        public void AddIfNotExists(Address address)
        {
            if (Exists(address))
                return;
            else
            {
                var sql = @"INSERT INTO [Address] (Country,PostCode,HouseNumber,City,Street) VALUES
                        (@country, @postCode, @houseNumber, @city, @Street)";
                using (var cmd = new SqlCommand(sql, con.connection))
                {
                    cmd.Parameters.Add("@country", SqlDbType.VarChar);
                    cmd.Parameters["@country"].Value = address.Country;

                    cmd.Parameters.Add("@postCode", SqlDbType.VarChar);
                    cmd.Parameters["@postCode"].Value = address.PostCode;

                    cmd.Parameters.Add("@houseNumber", SqlDbType.VarChar);
                    cmd.Parameters["@houseNumber"].Value = address.HouseNumber;

                    cmd.Parameters.Add("@city", SqlDbType.VarChar);
                    cmd.Parameters["@city"].Value = address.City;

                    cmd.Parameters.Add("@street", SqlDbType.VarChar);
                    cmd.Parameters["@street"].Value = address.Street;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
