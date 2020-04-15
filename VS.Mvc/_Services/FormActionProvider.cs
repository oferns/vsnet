namespace VS.Mvc._Services {
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class FormActionProvider : IActionProvider {
        private readonly EndpointDataSource endpoints;

        public FormActionProvider(EndpointDataSource endpoints) {
            this.endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        }

        public ValueTask<IEnumerable<ContextualLink>> Actions(string formName, CancellationToken cancel) {
            var tes = new Dictionary<string, string> { 
                { "Controller", "LogIn" },
                { "Action","Index" }            
            };

            return new ValueTask<IEnumerable<ContextualLink>>(new[] { new ContextualLink("Log In", "Post", tes) });
        }
    }
}
