namespace VS.Abstractions.Data {
    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface to represent a transactional database client.
    /// Forces implementation of IDisposable becaise it exposes
    /// two properties from the System.Data namespace whose types implement IDisposable
    /// </summary>    
    public interface IDbClient : IDisposable {

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        ValueTask BeginTransaction(CancellationToken cancel);

        ValueTask Commit(CancellationToken cancel);

        ValueTask Rollback(CancellationToken cancel);

    }
}
