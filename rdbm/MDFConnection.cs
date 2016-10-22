using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public sealed class MDFConnection :IDisposable
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


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Close();
                    connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MDFConnection() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
