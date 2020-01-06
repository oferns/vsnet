
namespace VS.PostGres {
    using MediatR;
    using Npgsql;
    using System;
    using System.Data;
    using VS.Abstractions.Data;

    /// <summary>
    /// The PostGres SQL client.
    /// </summary>
    public sealed class PgClient : IDbClient {

        private readonly IDbClient wrappedClient;
        private readonly IMediator mediator;

        private bool attached = false;
        private readonly object atlock = new object();

        public PgClient(IDbClient wrappedClient, IMediator mediator) {
            this.wrappedClient = wrappedClient ?? throw new ArgumentNullException(nameof(wrappedClient));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public IDbConnection Connection { 
            get {
                if (!attached && wrappedClient.Connection is NpgsqlConnection conn) {
                    lock (atlock) {
                        if (!attached) {
                            conn.Notice += async (s, e) => {
                                await mediator.Publish(new PgMessageNotification(e.Notice));
                            };
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
