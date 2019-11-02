namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Mvc._Extensions;

    public static class Routing {


        public static IServiceCollection AddConstraints(this IServiceCollection services) {

            return services.AddRouting(o => {
                o.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                
            });
        }

        public static IEndpointRouteBuilder AddMvcEndpoints(this IEndpointRouteBuilder builder) {

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

            builder.MapControllerRoute(
                name: "DefaultRoute",
                pattern: "",
                defaults: new { controller = "Home", Action = "Index" });



            return builder;

        }



    }
}
