
namespace VS.Mvc.Services.DevOnly {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public class NoOpTransaction : IDbTransaction {

        public NoOpTransaction(IDbConnection connection) {
            Connection = connection;
        }    

        public IsolationLevel IsolationLevel => IsolationLevel.Unspecified;

        public IDbConnection Connection { get; }

        public void Commit() {
            
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {

        }

        public void Rollback() {
            
        }
    }
}
