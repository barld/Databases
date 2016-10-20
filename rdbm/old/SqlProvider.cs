using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace rdbm
{
    internal class SqlProvider : IDisposable
    {

        SqlConnection _connection;

        public SqlProvider(string connectionString)
        {
            this._connection = new MDFConnection().connection;
            OpenConnection();
        }

        private void OpenConnection()
        {
            switch (this._connection.State)
            {
                case System.Data.ConnectionState.Closed:
                    this._connection.Open();
                    break;

                case System.Data.ConnectionState.Broken:
                    this._connection.Close();
                    this._connection.Open();
                    break;
            }
        }

        internal IEnumerable<T> RunQuery<T>(string query)
        {
            using (var command = new SqlCommand(query, this._connection))
            {
                var reader = command.ExecuteReader();

                return new DataMapper<T>(reader);
            }
        }

        public void Dispose()
        {
            this._connection.Close();
        }
    }
}