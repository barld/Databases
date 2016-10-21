using EmployeDataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

                using (var reader = cmd.ExecuteReader())
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
                HourFee = (float) reader.GetSqlMoney(2).ToDouble(),
                BSN = reader.GetInt32(3)
            };
        }

        internal void Add(Position position)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();

            var sql = @"INSERT INTO [Position](PositionName, Description, HourFee, BSN) 
                VALUES(@pName, @description, @hourFee, @BSN);";
            using(var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@pName", SqlDbType.VarChar);
                cmd.Parameters["@pName"].Value = position.PositionName;
                cmd.Parameters.Add("@description", SqlDbType.Text);
                cmd.Parameters["@description"].Value = position.Description;
                cmd.Parameters.Add("@hourFee", SqlDbType.Money);
                cmd.Parameters["@hourFee"].Value = new  SqlMoney((double)position.HourFee);
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = position.BSN;

                cmd.ExecuteNonQuery();
            }
        }

        internal void DeleteFromBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"DELETE FROM [Position] WHERE BSN = @BSN;";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
