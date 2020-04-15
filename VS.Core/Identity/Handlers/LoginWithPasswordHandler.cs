namespace VS.Core.Identity.Handlers {

    using MediatR;
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginWithPasswordHandler : IRequestHandler<LoginWithPassword, ClaimsIdentity> {
        private readonly IMediator mediator;

        public LoginWithPasswordHandler(IMediator mediator) {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        
        
        public Task<ClaimsIdentity> Handle(LoginWithPassword request, CancellationToken cancellationToken) {

            // Check user


            // Check password

            // return user

            return Task.FromResult(new ClaimsIdentity());
        }
    }
}
