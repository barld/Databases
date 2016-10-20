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
    public class PositionGateWay
    {
        private readonly MDFConnection con;

        public PositionGateWay(MDFConnection connection)
        {
            con = connection;
        }

        public IEnumerable<Position> GetAllByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql = @"SELECT * FROM [Position] WHERE [Position].BSN = @BSN";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    yield return map(reader);

            }
        }

        private Position map(SqlDataReader reader)
        {
            return new Position
            {
                PositionName = reader.GetString(0),
                Description = reader.GetString(1),
                HourFee = reader.GetFloat(2),
                BSN = reader.GetInt32(3)
            };
        }
    }
}
