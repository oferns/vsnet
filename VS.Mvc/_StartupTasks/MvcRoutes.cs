namespace VS.Mvc.StartupTasks {
    
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class MvcRoutes {

        public static IEndpointRouteBuilder AddMvc(this IEndpointRouteBuilder builder) {

            // generic routes
            builder.MapControllerRoute(
                name: "SubAreaSubIdRoutes",
                pattern: "{area:exists:slugify}/{eid:int}/{subarea:exists:slugify}/{controller:slugify}/{childid:int}/{action:slugify=Item}"
            );

            builder.MapControllerRoute(
                name: "SubAreaSubRoutes",
                pattern: "{area:exists:slugify}/{subarea:exists:slugify}/{controller:slugify}/{action:slugify=Index}"
            );


            builder.MapControllerRoute(
               name: "SubAreaControllerIdRoutes",
               pattern: "{area:exists:slugify}/{eid:int}/{subarea:exists:slugify}/{controller:slugify}/{action:slugify=Index}"
            );

            builder.MapControllerRoute(
               name: "AreaSubIdRoutes",
               pattern: "{area:exists:slugify}/{eid:int}/{controller:slugify}/{childid:int}/{action:slugify=Item}"
            );

            builder.MapControllerRoute(
               name: "AreaIdRoutes",
               pattern: "{area:exists:slugify}/{eid:int}/{controller:slugify}/{action:slugify=Index}"
            );

            builder.MapControllerRoute(
               name: "AreaControllerIdRoutes",
               pattern: "{area:exists:slugify}/{controller:slugify}/{eid:int}/{action:slugify=Index}"
            );

            builder.MapControllerRoute(
               name: "AreaHome",
               pattern: "{area:exists:slugify}/{controller:slugify}/{action:slugify}",
               defaults: new { action = "Index" }
           );

            builder.MapControllerRoute(
                name: "ControllerItemRoutes",
                pattern: "{controller:slugify}/{eid:int}/{action:slugify=Item}"
            );

            builder.MapControllerRoute(
                 name: "DefaultAction",
                 pattern: "{controller:slugify}",
                 defaults: new { action = "Index" }
             );

            builder.MapControllerRoute(
                name: "DefaultHomeAction",
                pattern: "{action:slugify}",
                defaults: new { controller = "Home" }
            );

            builder.MapControllerRoute(
                name: "DefaultControllerAction",
                pattern: "{controller:slugify}/{action:slugify}"
            );


            return builder;

        }

    }
}
