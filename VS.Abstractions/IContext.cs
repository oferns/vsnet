namespace VS.Abstractions {
    using System.Globalization;
    using System.Security.Claims;
    using VS.Abstractions.Logging;

    public interface IContext {

        public ClaimsPrincipal User { get; }

        public string Host { get; }
       
        public CultureInfo UICulture { get; }

        public string RequestId { get; }

        public ILog Log { get; }
    }
}
