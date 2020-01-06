namespace VS.Abstractions.Data {
    using System;
    using System.Data;

    /// <summary>
    /// An interface to represent a transactional database client.
    /// Forces implementation of IDisposable becaise it exposes
    /// two properties from the System.Data namespace whose types implement IDisposable
    /// </summary>    
    public interface IDbClient : IDisposable {

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();

    }
}
