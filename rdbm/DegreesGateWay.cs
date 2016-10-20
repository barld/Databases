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
    public class DegreesGateWay
    {
        private readonly MDFConnection con;

        public DegreesGateWay(MDFConnection connection)
        {
            con = connection;
        }
        private Degree map(SqlDataReader reader)
        {
            return new Degree
            {
                Course = reader.GetString(0),
                School = reader.GetString(1),
                Level = reader.GetString(2),
                BSN = reader.GetInt32(3)
            };
        }
        public IEnumerable<Degree> GetAllByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"SELECT * FROM [Degree] WHERE [Degree].BSN = @BSN";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                        yield return map(reader);
                }
            }
        }

        public void Add(Degree degree)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"INSERT INTO [Degree] VALUES (@course, @school, @level, @BSN);";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@course", SqlDbType.VarChar);
                cmd.Parameters["@course"].Value = degree.Course;
                cmd.Parameters.Add("@school", SqlDbType.VarChar);
                cmd.Parameters["@school"].Value = degree.School;
                cmd.Parameters.Add("@level", SqlDbType.VarChar);
                cmd.Parameters["@level"].Value = degree.Level;
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = degree.BSN;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteByBSN(int BSN)
        {
            if (con.connection.State != ConnectionState.Open)
                con.connection.Open();
            var sql = @"DELETE FROM [Degree] WHERE [Degree].BSN = @BSN";
            using (var cmd = new SqlCommand(sql, con.connection))
            {
                cmd.Parameters.Add("@BSN", SqlDbType.Int);
                cmd.Parameters["@BSN"].Value = BSN;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
