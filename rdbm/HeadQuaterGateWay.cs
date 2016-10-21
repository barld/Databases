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
    public class HeadQuaterGateWay
    {
        private readonly MDFConnection con;
        private readonly IContext context;

        public HeadQuaterGateWay(MDFConnection connection, IContext context)
        {
            con = connection;
            this.context = context;
        }

        public IEnumerable<HeadQuater> GetAll()
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            using (var cmd = new SqlCommand("Select * From [HeadQuater]", con.connection))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return map(reader);
                }
            }
        }

        private HeadQuater map(SqlDataReader reader)
        {
            return new HeadQuater
            {
                BuildingName = reader.GetString(0),
                Rooms = reader.GetInt32(1),
                Rent = (float)reader.GetSqlMoney(2).ToDouble(),
                Country = reader.GetString(3),
                PostCode = reader.GetString(4),
                HouseNumber = reader.GetString(5),
            };
        }

        private HeadQuater fullMap(SqlDataReader reader)
        {
            return new HeadQuater
            {
                BuildingName = reader.GetString(0),
                Rooms = reader.GetInt32(1),
                Rent = (float)reader.GetSqlMoney(2).ToDouble(),
                Country = reader.GetString(3),
                PostCode = reader.GetString(4),
                HouseNumber = reader.GetString(5),
                Address = new Address
                {
                    Country = reader.GetString(3),
                    PostCode = reader.GetString(4),
                    HouseNumber = reader.GetString(5),
                    City = reader.GetString(9),
                    Street = reader.GetString(10)
                }
            };
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

        public HeadQuater FindByBuildingname(string buildingName)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql =
                    @"  SELECT [HeadQuater].*, [Address].* 
                        FROM [HeadQuater]
                        LEFT JOIN [Address] ON 
                        [HeadQuater].[Country] = [Address].[Country] AND
                        [HeadQuater].[PostCode] = [Address].[PostCode] AND
                        [HeadQuater].[HouseNumber] = [Address].[HouseNumber]
                        WHERE [HeadQuater].BuildingName = @buildingName";
            HeadQuater HQ;

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@buildingName", SqlDbType.VarChar);
                cmd.Parameters["@buildingName"].Value = buildingName;
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    HQ = fullMap(reader);
                }

            }
            return HQ;
        }

        public void DeleteByBuildingname(string buildingName)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql = @"DELETE FROM [HeadQuater] WHERE [HeadQuater].[BuildingName] = @buildingname";

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@buildingname", SqlDbType.VarChar);
                cmd.Parameters["@buildingname"].Value = buildingName;

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(HeadQuater headquarter)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            context.Addresses.AddIfNotExists(headquarter.Address);

            var sql = @"INSERT INTO [HeadQuater] 
                        VALUES (@buildingname, @rooms, @rent, @country, @postcode, @housenumber)";

            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@buildingname", SqlDbType.VarChar);
                cmd.Parameters["@buildingname"].Value = headquarter.BuildingName;

                cmd.Parameters.Add("@rooms", SqlDbType.Int);
                cmd.Parameters["@rooms"].Value = headquarter.Rooms;

                cmd.Parameters.Add("@rent", SqlDbType.Money);
                cmd.Parameters["@rent"].Value = headquarter.Rent;

                cmd.Parameters.Add("@country", SqlDbType.VarChar);
                cmd.Parameters["@country"].Value = headquarter.Country;

                cmd.Parameters.Add("@postcode", SqlDbType.VarChar);
                cmd.Parameters["@postcode"].Value = headquarter.PostCode;

                cmd.Parameters.Add("@housenumber", SqlDbType.VarChar);
                cmd.Parameters["@housenumber"].Value = headquarter.HouseNumber;

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(HeadQuater headquarter)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql = @"UPDATE [HeadQuater] SET 
                            BuildingName = @buildingname,
                            Rooms = @rooms,
                            Rent = @rent,
                            Country = @country,
                            PostCode = @postcode,
                            HouseNumber = @housenumber
                        WHERE BuildingName = @buildingname
                        ";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@buildingname", SqlDbType.VarChar);
                cmd.Parameters["@buildingname"].Value = headquarter.BuildingName;

                cmd.Parameters.Add("@rooms", SqlDbType.Int);
                cmd.Parameters["@rooms"].Value = headquarter.Rooms;

                cmd.Parameters.Add("@rent", SqlDbType.Money);
                cmd.Parameters["@rent"].Value = headquarter.Rent;

                cmd.Parameters.Add("@country", SqlDbType.VarChar);
                cmd.Parameters["@country"].Value = headquarter.Country;

                cmd.Parameters.Add("@postcode", SqlDbType.VarChar);
                cmd.Parameters["@postcode"].Value = headquarter.PostCode;

                cmd.Parameters.Add("@housenumber", SqlDbType.VarChar);
                cmd.Parameters["@housenumber"].Value = headquarter.HouseNumber;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
