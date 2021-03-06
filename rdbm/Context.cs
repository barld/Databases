﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdbm
{
    public interface IContext: IDisposable
    {
        EmployeeGateWay Employees { get; }
        HeadQuaterGateWay HeadQuaters { get; }
        DegreesGateWay Degrees { get; }
        PositionGateWay Positions { get; }
        AddressGateWay Addresses { get; }
        ProjectGateWay Projects { get; }
    }


    public class Context : IContext
    {
        public EmployeeGateWay Employees { get; }
        public HeadQuaterGateWay HeadQuaters { get; }
        public DegreesGateWay Degrees { get; }
        public PositionGateWay Positions { get; }
        public AddressGateWay Addresses { get; }
        public ProjectGateWay Projects { get; }

        public Context()
        {
            connection = new MDFConnection();

            Employees = new EmployeeGateWay(connection, this);
            HeadQuaters = new HeadQuaterGateWay(connection, this);
            Degrees = new DegreesGateWay(connection);
            Positions = new PositionGateWay(connection);
            Addresses = new AddressGateWay(connection);
            Projects = new ProjectGateWay(connection, this);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        private readonly MDFConnection connection;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Context() {
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
