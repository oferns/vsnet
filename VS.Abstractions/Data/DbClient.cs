namespace VS.Abstractions.Data {

    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A generic database client that can be used with any IDbConnection
    /// that manages multiple calls to BeginTransaction and correlates them
    /// to 
    /// </summary>
    public class DbClient : IDbClient {

        private readonly IDbConnection connection;
        protected internal int transactionCount;
        protected internal int commitCount;

        public DbClient(IDbConnection connection) {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public IDbTransaction Transaction { get; protected internal set; }
        public IDbConnection Connection {
            get {
                if (this.connection.State != ConnectionState.Open) {
                    connection.Open();
                }
                return connection;
            }
        }

        public virtual ValueTask BeginTransaction(CancellationToken cancel) {
            transactionCount++;
            if (Transaction is null) {
                Transaction = this.Connection.BeginTransaction();
            }
            return new ValueTask();
        }
        public virtual ValueTask Commit(CancellationToken cancel) {
            commitCount++;
            if (commitCount.Equals(transactionCount)) {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            return new ValueTask();

        }

        public virtual ValueTask Rollback(CancellationToken cancel) {
            if (Transaction is object) {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
            return new ValueTask();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {

            if (disposing) {
#if DEBUG
                if (this.commitCount != this.transactionCount) {
                    System.Diagnostics.Debug.WriteLine("You have left a DB transaction open. Look for a call to Begin() without a corresponding Commit(). This transaction will be rolled back");
                    Rollback(CancellationToken.None);
                }
#endif
                if (this.connection is object) {
                    this.connection.Dispose(); // Equivelent to Close() 
                }
            }
        }
    }
}
