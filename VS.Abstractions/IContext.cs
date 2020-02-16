namespace VS.Abstractions {
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Claims;
    using Microsoft.Extensions.Primitives;

    public interface IContext {

        public ClaimsPrincipal User { get; }

        public string Host { get; }
       
        public CultureInfo UICulture { get; }

        public string RequestId { get; }


        IEnumerable<KeyValuePair<string, StringValues>> Query { get; }

    }
}
