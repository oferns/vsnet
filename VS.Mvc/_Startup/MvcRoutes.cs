namespace VS.Mvc._Startup {

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using VS.Mvc._Extensions;

    public static class Routing {


        public static IServiceCollection AddConstraints(this IServiceCollection services) {

            return services.AddRouting(o => {
                o.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                o.LowercaseQueryStrings = true;
                o.LowercaseUrls = true;
                o.AppendTrailingSlash = false;
            });
        }

        public static IEndpointRouteBuilder AddMvcEndpoints(this IEndpointRouteBuilder builder) {

            // generic routes
            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{area:exists:slugify}/{eid:int}/{subarea:exists:slugify}/{controller:slugify}/{childid:int}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
                "{area:exists:slugify}/{subarea:exists:slugify}/{controller:slugify}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{area:exists:slugify}/{eid:int}/{subarea:exists:slugify}/{controller:slugify}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{area:exists:slugify}/{eid:int}/{controller:slugify}/{childid:int}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{area:exists:slugify}/{eid:int}/{controller:slugify}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(               
               "{area:exists:slugify}/{controller:slugify}/{eid:int}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{area:exists:slugify}/{controller:slugify}/{action:slugify}"
           );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{controller:slugify}/{eid:int}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{controller:slugify}"
             );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
               "{controller:slugify}/{action:slugify}"
            );

            builder.MapDynamicControllerRoute<LocalizedRouteValueTransformer>(
                "{controller=Home}/{action=Index}"
             );

         //   builder.MapControllerRoute(
         //       name: "DefaultRoute",
         //       pattern: "",
         //       defaults: new { controller = "Home", Action = "Index" });



            return builder;

        }
    }
}
