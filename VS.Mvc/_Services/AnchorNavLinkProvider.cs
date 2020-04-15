namespace VS.Mvc._Services {
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class AnchorNavLinkProvider : INavLinkProvider {
        private readonly EndpointDataSource endpoints;

        public AnchorNavLinkProvider(EndpointDataSource endpoints) {
            this.endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public ValueTask<IEnumerable<ContextualLink>> Links(string groupName, CancellationToken cancel) {

            dynamic tes = new ExpandoObject();

            tes.Controller = "Login";
            tes.Action = "Index";

            return new ValueTask<IEnumerable<ContextualLink>>(new[] { new ContextualLink("Login", "Get", tes) });


        }
    }
}
