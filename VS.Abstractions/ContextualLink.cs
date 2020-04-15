namespace VS.Abstractions {

    using System.Collections.Generic;

    public class ContextualLink {

        public ContextualLink(string title, string method, IDictionary<string, string> routeValues) {
            Title = title;
            Method = method;
            RouteValues = routeValues;
        }


        public string Title { get; private set; }

        public string Method { get; private set; }

        public IDictionary<string, string> RouteValues { get; private set; }

    }
}