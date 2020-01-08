namespace VS.Mvc.Services.DevOnly {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public class NoOpConnection : IDbConnection {
        public string ConnectionString { get; set; }

        public int ConnectionTimeout => 100;

        public string Database => string.Empty;

        public ConnectionState State { get; private set; } = ConnectionState.Closed;

        public IDbTransaction BeginTransaction() {
            return new NoOpTransaction(this);
        }

        public void Open() {
            this.State = ConnectionState.Open;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il) {
            return new NoOpTransaction(this);
        }

        public void ChangeDatabase(string databaseName) {
            
        }

        public void Close() {
            this.State = ConnectionState.Closed;
        }

        public IDbCommand CreateCommand() {
            return null;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {

        }
    }

}
