using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace rdbm
{
    internal class DataMapper<T> : IEnumerable<T>, IDisposable
    {
        private SqlDataReader _reader;

        public DataMapper(SqlDataReader reader)
        {
            this._reader = reader;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var type = typeof(T);

            if (type.IsValueType || type == typeof(string))
                return this.getValueTypeCollection().GetEnumerator();
            else
                return this.getReferenceTypeCollection().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerable<T> getValueTypeCollection()
        {
            while (_reader.Read())
            {
                var val = this._reader[0];

                yield return (T)val;
            }
        }

        IEnumerable<T> getReferenceTypeCollection()
        {

            //object properties
            var objProps = typeof(T).GetProperties();

            //column properties
            var schemaTable = this._reader.GetSchemaTable();
            var list = new List<string>();

            foreach (DataRow colNames in schemaTable.Rows)
            {
                list.Add(colNames[0].ToString());
            }

            var colProps = objProps.Where(x => list.Contains(x.Name));


            while (this._reader.Read())
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var prop in colProps)
                {
                    var colName = prop.Name;

                    var colObjValue = this._reader[colName];

                    prop.SetValue(obj, colObjValue is DBNull ? null : colObjValue);
                }

                yield return obj;
            }
        }

        public void Dispose()
        {
            if (!_reader.IsClosed)
                _reader.Close();
        }
    }
}