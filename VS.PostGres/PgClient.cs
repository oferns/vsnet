
namespace VS.PostGres {

    using Npgsql;
    using System;
    using System.Data;
    using VS.Abstractions.Data;


    /// <summary>
    /// The PostGres SQL client.
    /// </summary>
    public sealed class PgClient : IDbClient {

        public event NoticeEventHandler MessageRecieved;
        private readonly IDbClient wrappedClient;

        private bool attached = false;
        private readonly object atlock = new object();

        public PgClient(IDbClient wrappedClient) {
            this.wrappedClient = wrappedClient ?? throw new ArgumentNullException(nameof(wrappedClient));
        }

        public IDbConnection Connection {
            get {
                if (!attached && MessageRecieved is object && wrappedClient.Connection is NpgsqlConnection conn) {
                    lock (atlock) {
                        if (!attached) {
                            conn.Notice += MessageRecieved;
                        }
                        attached = true;
                    }
                } else {
                    attached = true;
                }

                return wrappedClient.Connection;
            }
        }

        public IDbTransaction Transaction => this.wrappedClient.Transaction;


        public void BeginTransaction() {
            this.wrappedClient.BeginTransaction();
        }

        public void Commit() {
            this.wrappedClient.Commit();
        }

        public void Dispose() {
            this.wrappedClient.Dispose();
        }

        public void Rollback() {
            this.wrappedClient.Rollback();
        }
    }
}
