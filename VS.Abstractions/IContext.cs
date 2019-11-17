namespace VS.Abstractions {

    using System.Security.Claims;

    public interface IContext {

        public ClaimsPrincipal User { get; }


        public string Host { get; }


        public string RequestId { get; }
        
    }
}
