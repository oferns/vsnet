namespace VS.Core.PostGres {
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Npgsql;
    using VS.Abstractions.Data;

    public class PgMessageHandler : IDbMessageHandler<NpgsqlNoticeEventArgs> {
        private readonly IMediator mediator;

        public PgMessageHandler(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task MessageRecieved(object sender, NpgsqlNoticeEventArgs args) {
            return mediator.Send(new PgMessageNotification(args.Notice));
        }
    }
}
                                                                                              