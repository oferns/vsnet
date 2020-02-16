namespace VS.Mvc._Middleware.DevOnly {

    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Routing.Internal;

    public class RouteGraphMiddleware : IMiddleware {

        private readonly DfaGraphWriter _graphWriter;
        private readonly EndpointDataSource _endpointDataSource;

        public RouteGraphMiddleware(DfaGraphWriter graphWriter, EndpointDataSource endpointDataSource) {
            _graphWriter = graphWriter ?? throw new ArgumentNullException(nameof(graphWriter));
            _endpointDataSource = endpointDataSource ?? throw new ArgumentNullException(nameof(endpointDataSource));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
           
            using var sw = new StringWriter();
            _graphWriter.Write(_endpointDataSource, sw);
            await context.Response.WriteAsync(sw.ToString());
            
        }
    }
}
