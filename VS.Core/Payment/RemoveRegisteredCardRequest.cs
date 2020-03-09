
namespace VS.Core.Payment {
    using System;
    using MediatR;

    public class RemoveRegisteredCardRequest : IRequest<bool> {

        public RemoveRegisteredCardRequest(string referenceId, string userId) {
            ReferenceId = referenceId;
            UserId = userId;
        }

        public string ReferenceId { get; }
        public string UserId { get; }
    }
}
