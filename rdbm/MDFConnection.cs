using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public class MDFConnection
    {
        public SqlConnection connection { get; }

        public MDFConnection(string file)
        {
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename={file};
                          Integrated Security=True;
                          Connect Timeout=30;
                          Integrated Security=True";
            connection = new SqlConnection(connectionString);
        }

        public MDFConnection() : this(System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath.Replace("WebApplication\\bin",""), @"rdbm\Employe.mdf")) { }
    }
}
