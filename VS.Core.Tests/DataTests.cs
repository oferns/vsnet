namespace VS.Core.Tests {

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using VS.Core.Data.Abstractions;
    using VS.Core.Data.Filtering;
    using VS.Core.Data.Paging;

    [TestClass]
    public class DataTests {

        [TestClass]
        public class FilterTests {

            [TestMethod]
            public void ShouldBeReadOnly() {
                Assert.IsTrue(new Filter<TestModel>().IsReadOnly);
            }

            [TestMethod]
            public void ShouldReturnFalseWhenRemoveIsCalled() {
                Assert.IsFalse(new Filter<TestModel>().Remove(default));
            }

            [TestMethod]
            public void ShouldBeEqual() {

                var filter = new Filter<TestModel> {
                    new Clause<TestModel>("StringProp", EvalOp.Equals, "test")
                };

                var filter1 = new Filter<TestModel> {
                    new Clause<TestModel>("StringProp", EvalOp.Equals, "test")
                };

                Assert.AreEqual(filter, filter1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenClausesAreDifferent() {

                var filter = new Filter<TestModel> {
                new Clause<TestModel>("StringProp", EvalOp.Equals, "test")
            };

                var filter1 = new Filter<TestModel> {
                new Clause<TestModel>("StringProp", EvalOp.Equals, "test1")
            };


                Assert.AreNotEqual(filter, filter1);

            }

            [TestMethod]
            public void ShouldNotBeEqualWhenValuesAreDifferent() {

                var filter = new Filter<TestModel> {
                new Clause<TestModel>("StringProp", EvalOp.Equals, "test")
            };

                var filter1 = new Filter<TestModel> {
                new Clause<TestModel>("StringProp", EvalOp.Equals, "test1")
            };

                Assert.AreNotEqual(filter, filter1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenOperatorsAreDifferent() {

                var filter = new Filter<TestModel> {
                new ClauseEntry<TestModel> (FilterOp.And, new Clause<TestModel>("StringProp", EvalOp.Equals, "test") as IClause<TestModel>)
            };

                var filter1 = new Filter<TestModel> {
                new ClauseEntry<TestModel> (FilterOp.Or, new Clause<TestModel>("StringProp", EvalOp.Equals, "test") as IClause<TestModel>)
            };

                Assert.AreNotEqual(filter, filter1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenPropertiesAreDifferent() {

                var filter = new Filter<TestModel> {
                new ClauseEntry<TestModel> (FilterOp.And, new Clause<TestModel>("StringProp", EvalOp.Equals, "test") as IClause<TestModel>)
            };

                var filter1 = new Filter<TestModel> {
                new ClauseEntry<TestModel> (FilterOp.And, new Clause<TestModel>("StringProp1", EvalOp.Equals, "test") as IClause<TestModel>)
            };

                Assert.AreNotEqual(filter, filter1);
            }

            [TestMethod]
            public void ShouldAcceptFilter() {

                var filter = new Filter<TestModel> {
                (IClause<TestModel>) new Filter<TestModel> {
                    new Clause<TestModel>("StringProp", EvalOp.Equals, "test")
                }
            };
            }
        }

        [TestClass]
        public class ClauseTests {

            [TestMethod]
            public void ShouldBeEqual() {
                var clause = new Clause<TestModel>("StringProp", EvalOp.Equals, "test");
                var clause1 = new Clause<TestModel>("StringProp", EvalOp.Equals, "test");
                Assert.AreEqual(clause, clause1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenPropertiesAreDifferent() {
                var clause = new Clause<TestModel>("StringProp", EvalOp.Equals, "test");
                var clause1 = new Clause<TestModel>("StringProp", EvalOp.Equals, "test1");
                Assert.AreNotEqual(clause, clause1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenPropertyNamesAreDifferent() {
                var clause = new Clause<TestModel>("StringProp", EvalOp.Equals, "test");
                var clause1 = new Clause<TestModel>("StringProp1", EvalOp.Equals, "test");
                Assert.AreNotEqual(clause, clause1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenOperatorsAreDifferent() {
                var clause = new Clause<TestModel>("StringProp", EvalOp.NotEqual, "test");
                var clause1 = new Clause<TestModel>("StringProp", EvalOp.Equals, "test");
                Assert.AreNotEqual(clause, clause1);
            }

            [TestMethod]
            public void ShouldNotBeEqualWhenWrongObjectType() {
                var clause = new Clause<TestModel>("StringProp", EvalOp.NotEqual, "test");
                Assert.IsFalse(clause.Equals(new object()));
            }
        }


        [TestClass]
        public class PagedListTests {

            [TestMethod]
            public void ShouldCalculateCorrectPageCount() {
                // Arrange && Act
                var model = new PagedList<object>(Enumerable.Empty<object>(), 1, 25, 59197);

                // Asssert
                Assert.AreEqual(model.PageCount, 2368);
            }

            [TestMethod]
            public void ShouldCalculateCorrectLastPage() {
                // Arrange && Act
                var model = new PagedList<object>(Enumerable.Empty<object>(), 59172, 25, 59197);

                // Asssert
                Assert.AreEqual(2368, model.CurrentPage);
            }

            [TestMethod]
            public void ShouldCalculateCorrectPenultimatePage() {
                // Arrange && Act
                var model = new PagedList<object>(Enumerable.Empty<object>(), 89, 10, 100);

                // Asssert
                Assert.AreEqual(9, model.CurrentPage);
            }

            [TestMethod]
            public void ShouldCalculateCorrect2ndPage() {
                // Arrange && Act
                var model = new PagedList<object>(Enumerable.Empty<object>(), 25, 25, 59197);

               // Asssert
                Assert.AreEqual(2, model.CurrentPage);
            }

            [TestMethod]
            public void ShouldCalculateCorrectFirstPage() {
                // Arrange && Act
                var model = new PagedList<object>(Enumerable.Empty<object>(), 0, 25, 59197);
                // Asssert
                Assert.AreEqual(1, model.CurrentPage);
            }
        }
    }
}