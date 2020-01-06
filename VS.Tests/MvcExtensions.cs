namespace VS.Tests {

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
                    var expectedResult = new[] { "{0}/_fr/FR/{1}.test", "{0}/_fr/{1}.test", "{0}/{1}.test", "{0}.test" };

                    // Act
                    var result = expander.ExpandViewLocations(context, new[] { "{0}/{1}.test", "{0}.test" });

                    // Assert
                    Assert.AreEqual(expectedResult.Length, result.Count());
                    Assert.AreEqual(expectedResult[0], result.ToArray<string>()[0]);
                    Assert.AreEqual(expectedResult[1], result.ToArray<string>()[1]);
                    Assert.AreEqual(expectedResult[2], result.ToArray<string>()[2]);
                    Assert.AreEqual(expectedResult[3], result.ToArray<string>()[3]);
                }
            }
        }
    }
}
