namespace VS.Core.Messaging.Queue {

    using MediatR;
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;
    using VS.Core.Storage;

    public class ConsumeHandler<T> : IRequestHandler<Consume<T>, T> where T : class {

        private readonly IMediator mediator;
        private readonly IContext context;
        private readonly ISerializer serializer;

        public ConsumeHandler(IMediator mediator, IContext context, ISerializer serializer) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<T> Handle(Consume<T> request, CancellationToken cancel) {
            using var stream = new MemoryStream();
            await serializer.SerializeToStream<T>(request.Message.Body, stream, cancel);
            stream.Seek(0, SeekOrigin.Begin);
            await mediator.Send(new Put(new Uri($"{context.Host}/divertedmessages/{request.Message.CorrelationId}.message", UriKind.Relative), stream, new ContentDisposition(DispositionTypeNames.Attachment), new ContentType()), cancel);
            return request.Message.Body;
        }
    }
}
