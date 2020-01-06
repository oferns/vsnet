namespace VS.Core.Data {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using VS.Abstractions.Data;


    /// <summary>
    /// A generic database client that can be used with any IDbConnection
    /// that manages multiple calls to BeginTransaction and correlates them
    /// to 
    /// </summary>
    public class DbClient : IDbClient {

        private readonly IDbConnection connection;
        private int transactionCount;
        private int commitCount;

        public DbClient(IDbConnection connection) {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public IDbTransaction Transaction { get; private set; }
        public IDbConnection Connection {
            get {
                if (this.connection.State != ConnectionState.Open) {
                    connection.Open();
                }
                return connection;
            }
        }
        
        public void BeginTransaction() {
            transactionCount++;
            if (Transaction == null) {
                Transaction = this.Connection.BeginTransaction();
            }
        }
        public void Commit() {
            commitCount++;
            if (commitCount == transactionCount) {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Rollback() {
            if (Transaction != null) {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
        }


        public void Dispose() {
#if DEBUG
            if (this.commitCount != this.transactionCount) {
                System.Diagnostics.Debug.WriteLine("You have left a DB transaction open. Look for a call to Begin() without a corresponding Commit(). This transaction will be rolled back");
                Rollback();
            }
#endif
            if (this.connection != null) {
                this.connection.Dispose(); // Equivelent to Close() 
            }
        }
    }
}
