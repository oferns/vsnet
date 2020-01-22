namespace VS.Core.PostGres {

    using System;
    using MediatR;
    using Npgsql;
    using VS.Abstractions.Data;

    /// <summary>
    /// A class that implements INotification to allow the mediator to publish
    /// messages from the postgres connection (raised in PL/SQL by using the RAISE statement) 
    /// </summary>
    public class PgMessageNotification : IDbMessage<PostgresNotice>, INotification {

        public PgMessageNotification(PostgresNotice message) {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public PostgresNotice Message { get; }
    }
}
