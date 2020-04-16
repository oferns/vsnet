namespace VS.Mvc._Extensions {
    using System;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using VS.Mvc.Api;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DataRouteConventionAttribute : Attribute, IControllerModelConvention {

        public void Apply(ControllerModel controller) {
            if (controller is null) {
                throw new ArgumentNullException(nameof(controller));
            }

            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(ApiController<>)) {
                var entityType = controller.ControllerType.GenericTypeArguments[0];
                controller.RouteValues["typename"] = entityType.Name;
                controller.ControllerName = "Data";
            }
        }

    }
}