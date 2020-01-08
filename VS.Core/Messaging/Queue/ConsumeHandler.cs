namespace VS.Core.Messaging.Queue {

    using MediatR;
    using MessagePack;
    using MessagePack.Resolvers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions.Messaging;
    using VS.Core.Storage;

    public class ConsumeHandler<T> : IRequestHandler<Consume<T>, T> where T : class {

        private readonly IMediator mediator;

        public ConsumeHandler(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<T> Handle(Consume<T> request, CancellationToken cancel) {
            using var stream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync<T>(stream, request.Message.Body, ContractlessStandardResolver.Options, cancel);
            stream.Seek(0, SeekOrigin.Begin);
            await mediator.Send(new Put(new Uri($"messages/{request.Message.CorrelationId}.queue", UriKind.Relative), stream, new ContentDisposition(DispositionTypeNames.Attachment), new ContentType()), cancel);
            return request.Message.Body;
        }
    }
}
