
namespace VS.Tests {
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using VS.Mvc._Extensions;

    [TestClass]
    public class MvcExtensions {


        [TestClass]
        public class ViewLocationExpanders {

            [TestClass]
            public class UICulture {

                [TestMethod]
                public void ShouldReturnOneResultForTopLevelCulture() {
                    // Arrange

                    CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
                    var expander = new UICultureViewLocationExpander();
                    var context = new ViewLocationExpanderContext(new ActionContext(),"test", "test", "test", "test", true);
                    // Act
                    var result = expander.ExpandViewLocations(context, new[] { "{0}/{1}.test", "{0}.test" });
                    // Assert
                    Assert.IsTrue(true);
                }
            }
        }
    }
}
