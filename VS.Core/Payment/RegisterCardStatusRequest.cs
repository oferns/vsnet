namespace VS.Core.Payment {

    using MediatR;

    public class RegisterCardStatusRequest : IRequest<RegisterCardStatusResponse> {

        public RegisterCardStatusRequest(string registrationId) {
            RegistrationId = registrationId;
        }

        public string RegistrationId { get; private set; }
    }
}
