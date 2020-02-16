namespace VS.Tests {

    using System.Globalization;    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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

                    var resultsEnum = result.GetEnumerator();

                    Assert.IsTrue(resultsEnum.MoveNext());
                    Assert.AreEqual(expectedResult[0], resultsEnum.Current);

                    Assert.IsTrue(resultsEnum.MoveNext());
                    Assert.AreEqual(expectedResult[1], resultsEnum.Current);

                    Assert.IsTrue(resultsEnum.MoveNext());
                    Assert.AreEqual(expectedResult[2], resultsEnum.Current);

                    Assert.IsTrue(resultsEnum.MoveNext());
                    Assert.AreEqual(expectedResult[3], resultsEnum.Current);

                    Assert.IsFalse(resultsEnum.MoveNext());
                }
            }
        }
    }
}
