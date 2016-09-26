using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public class EmployeGateWay
    {
        public static void GetAll()
        {
            var dt = new DataTable();
            //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\barld\documents\visual studio 2015\Projects\rdbm\rdbm\Employe.mdf";Integrated Security=True
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=C:\Users\barld\documents\visual studio 2015\Projects\rdbm\rdbm\Employe.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          Integrated Security=True";
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("Select * From [Employe]", cn))
            {
                cn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
    }
}
