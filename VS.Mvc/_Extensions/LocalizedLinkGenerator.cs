using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace VS.Mvc._Extensions {
    public class LocalizedLinkGenerator : LinkGenerator {
        public LocalizedLinkGenerator() {
        }

        public override string GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions options = null) {
            throw new NotImplementedException();
        }

        public override string GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions options = null) {
            throw new NotImplementedException();
        }

        public override string GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary ambientValues = null, string scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions options = null) {
            throw new NotImplementedException();
        }

        public override string GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions options = null) {
            throw new NotImplementedException();
        }
    }
}
