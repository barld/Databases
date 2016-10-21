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

        public HeadQuaterGateWay(MDFConnection connection)
        {
            con = connection;
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
                HouseNumber = reader.GetString(5)
            };
        }

        
    }
}
