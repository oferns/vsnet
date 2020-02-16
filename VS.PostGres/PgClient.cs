namespace VS.PostGres {

    using System.Data;
    using Npgsql;
    using VS.Abstractions.Data;

    /// <summary>
    /// The PostGres SQL client.
    /// </summary>
    public sealed class PgClient : DbClient {

        public event NoticeEventHandler MessageRecieved;
        public PgClient(IDbConnection connection) : base(connection) {
        }
    }
}
