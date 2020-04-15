
namespace VS.Mvc._Extensions {


    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;

    public abstract class FormController {

        private ControllerContext _controllerContext;
        private ViewDataDictionary _viewData;


        public ModelStateDictionary ModelState => this.ControllerContext.ModelState;

        /// <summary>
        /// Gets or sets the <see cref="Mvc.ControllerContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Controllers.IControllerActivator"/> activates this property while activating controllers.
        /// If user code directly instantiates a controller, the getter returns an empty
        /// <see cref="Mvc.ControllerContext"/>.
        /// </remarks>
        [ControllerContext]
        public ControllerContext ControllerContext {
            get => this._controllerContext ?? (_controllerContext = new ControllerContext());
            set => _controllerContext = value ?? throw new ArgumentNullException(nameof(value));
        }



    }
}
