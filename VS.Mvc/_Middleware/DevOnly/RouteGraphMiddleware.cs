
namespace VS.Mvc._Middleware.DevOnly {

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Routing.Internal;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

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
