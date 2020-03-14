namespace VS.Core {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions;
    using VS.Abstractions.Logging;

    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly IContext context;

        public LoggingBehaviour(IContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            context.Log.LogInfo($"Executing {typeof(TRequest).FullName}");
            var response = await next();
            context.Log.LogInfo($"Executed {typeof(TRequest).FullName}");

            return response;
        }
    }
}
