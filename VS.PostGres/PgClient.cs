namespace VS.PostGres {

    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Npgsql;
    using VS.Abstractions.Data;

    /// <summary>
    /// The PostGres SQL client.
    /// </summary>
    public sealed class PgClient : DbClient {

        private NpgsqlConnection NpgsqlConnection => this.Connection as NpgsqlConnection;
        private NpgsqlTransaction NpgsqlTransaction => this.Transaction as NpgsqlTransaction;


        public event NoticeEventHandler MessageRecieved;
        
        public PgClient(IDbConnection connection) : base(connection) { }

        public override async ValueTask BeginTransaction(CancellationToken cancel) {
            transactionCount++;
            if (Transaction is null) {
                Transaction = await NpgsqlConnection.BeginTransactionAsync(cancel);
            }            
        }

        public override async ValueTask Commit(CancellationToken cancel) {
            commitCount++;
            if (commitCount.Equals(transactionCount)) {
                await NpgsqlTransaction.CommitAsync(cancel);
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public async override ValueTask Rollback(CancellationToken cancel) {
            if (NpgsqlTransaction is object) {
                await NpgsqlTransaction.RollbackAsync(cancel);
                NpgsqlTransaction.Dispose();
                Transaction = null;
            }
        }
    }
}
