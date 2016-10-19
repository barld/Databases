using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public class DataContext : IDisposable
    {
        SqlProvider _provider;

        string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=C:\Users\Daan\Desktop\Databases\rdbm\Employe.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          Integrated Security=True";
        public DataContext()
        {
            this._provider = new SqlProvider(connectionString);
        }

        public IEnumerable<T> Query<T>(string query)
        {
            return this._provider.RunQuery<T>(query);

        }

        public void Dispose()
        {
            this._provider.Dispose();
        }
    }
}
