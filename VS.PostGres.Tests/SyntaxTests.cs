namespace VS.PostGres.Tests {
    using System.Collections.Generic;
    using System.Dynamic;    
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;
    using VS.Abstractions.Logging;
    using VS.Data.PostGres.App;
    using VS.Data.PostGres.App.Meta;

    [TestClass]
    public class SyntaxTests {

        internal Mock<ILog> log = new Mock<ILog>();

        [TestMethod]
        public void ShouldProduceSelectStatement() {

            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);

            // Act
            var sql = provider.Select(default, default, default, out var parameters);

            // Assert
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture;", sql);
        }


        [TestMethod]
        public void ShoulldProductSelectStatementWithArgs() {

            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);

            // Act
            var sql = provider.Select(default, default, default, out var parameters);



        }

        [TestMethod]
        public void ShouldProduceSelectWithSimpleFilter() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var filter = new Filter<Culture> { new ClauseEntry<Culture>(FilterOp.And, new Clause<Culture>("Code", EvalOp.Equals, "en")) };

            // Assert
            var sql = provider.Select(filter, default, default, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();

            Assert.AreEqual(1, parameters.Count);
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());
            Assert.AreEqual("Code0_0", keyEnum.Current);
            Assert.AreEqual("en", valueEnum.Current);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture WHERE code = @Code0_0;", sql);

        }

        [TestMethod]
        public void ShouldProduceSelectWithSimplePaging() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var pager = new Pager(0, 10);

            // Assert
            var sql = provider.Select(default, default, pager, out var parameters);

            // Act
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture LIMIT 10 OFFSET 0;", sql);

        }

        [TestMethod]
        public void ShouldProduceSelectWithSimpleSorting() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var sorter = new Sorter<Culture> { { "Code", true } };

            // Assert
            var sql = provider.Select(default, sorter, default, out var parameters);

            // Act
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture ORDER BY code ASC;", sql);
        }

        [TestMethod]
        public void ShouldProduceSelectWithSimpleFilteringAndSorting() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var filter = new Filter<Culture> { new ClauseEntry<Culture>(FilterOp.And, new Clause<Culture>("Code", EvalOp.Equals, "en")) };
            var sorter = new Sorter<Culture> { { "Code", true } };

            // Assert
            var sql = provider.Select(filter, sorter, default, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("Code0_0", keyEnum.Current);
            Assert.AreEqual("en", valueEnum.Current);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture WHERE code = @Code0_0 ORDER BY code ASC;", sql);
        }

        [TestMethod]
        public void ShouldProduceSelectWithSimpleFilteringAndPaging() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var filter = new Filter<Culture> { new ClauseEntry<Culture>(FilterOp.And, new Clause<Culture>("Code", EvalOp.Equals, "en")) };
            var pager = new Pager(0, 10);

            // Assert
            var sql = provider.Select(filter, default, pager, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("Code0_0", keyEnum.Current);
            Assert.AreEqual("en", valueEnum.Current);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture WHERE code = @Code0_0 LIMIT 10 OFFSET 0;", sql);
        }

        [TestMethod]
        public void ShouldProduceSelectWithSimplePagingAndSorting() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var pager = new Pager(0, 10);
            var sorter = new Sorter<Culture> { { "Code", true } };

            // Assert
            var sql = provider.Select(default, sorter, pager, out var parameters);

            // Act
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, specific_culture, english_name, local_name FROM app.culture ORDER BY code ASC LIMIT 10 OFFSET 0;", sql);
        }

        [TestMethod]
        public void ShouldProduceDynamicSelect() {
            // Arrange
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);

            var fields = new[] { "Code", "EnglishName" };

            // Act
            var sql = provider.Select(fields, default, default, default, out var parameters);

            // Assert
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, english_name FROM app.culture;", sql);
        }

        [TestMethod]
        public void ShouldLogInvalidPropertyNamesInDynamicSelect() {

            // Arrange
            var internalLog = new Mock<ILog>();
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), internalLog.Object);
            internalLog.Setup(l => l.Log(It.IsAny<LogEntry>())).Verifiable();
            var fields = new[] { "Code", "EnglishName", "Nonsense" };

            // Act
            var sql = provider.Select(fields, default, default, default(IPager), out var parameters);

            // Assert
            internalLog.Verify(l => l.Log(It.IsAny<LogEntry>()), Times.Once());
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, english_name FROM app.culture;", sql);


        }

        [TestMethod]
        public void ShouldReturnAllColumnsWhenNoRequestedColumsFound() {

            var internalLog = new Mock<ILog>();
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), internalLog.Object);
            internalLog.Setup(l => l.Log(It.IsAny<LogEntry>())).Verifiable();
            var fields = new[] { "Code", "EnglishName", "Nonsense" };

            // Act
            var sql = provider.Select(fields, default, default, default, out var parameters);

            // Assert
            internalLog.Verify(l => l.Log(It.IsAny<LogEntry>()), Times.Once());
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual("SELECT code, english_name FROM app.culture;", sql);

        }

        [TestMethod]
        public void ShouldReturnUpdateStatementWithSimplleFilter() {

            // Arrav
            var internalLog = new Mock<ILog>();
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), internalLog.Object);
            dynamic updateObject = new ExpandoObject();
            updateObject.Code = "bc";

            var filter = new Filter<Culture> { new ClauseEntry<Culture>(FilterOp.And, new Clause<Culture>("Code", EvalOp.Equals, "en")) };

            // Act
            var sql = provider.Update(updateObject, filter, out IDictionary<string, object> parameters);

            // Assert
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("Code0_0", keyEnum.Current);
            Assert.AreEqual("en", valueEnum.Current);

            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());
            
            Assert.AreEqual("@Code", keyEnum.Current);
            Assert.AreEqual("bc", valueEnum.Current);

            Assert.AreEqual("UPDATE app.culture SET code = @Code WHERE code = @Code0_0 RETURNING *;", sql);

        }

        [TestMethod]
        public void ShouldReturnDeleteStatementWithSimpleFilter() {
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);
            var filter = new Filter<Culture> { new ClauseEntry<Culture>(FilterOp.And, new Clause<Culture>("Code", EvalOp.Equals, "en")) };

            // Assert
            var sql = provider.Delete(filter, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("Code0_0", keyEnum.Current);
            Assert.AreEqual("en", valueEnum.Current);
            Assert.AreEqual("DELETE FROM app.culture WHERE code = @Code0_0 RETURNING *;", sql);

        }

        [TestMethod]
        public void ShouldReturnInsertStatementForOneEntity() {
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);

            var culture = new Culture { Code = "Test", EnglishName = "Test", LocalName = "Test", SpecificCulture = "Test" };
            // Assert
            var sql = provider.Insert(new[] { culture }, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(4, parameters.Count);
            Assert.AreEqual("Code0", keyEnum.Current);
            Assert.AreEqual("Test", valueEnum.Current);

            // TODO: The rest of the parameters
            Assert.AreEqual("INSERT INTO app.culture (code, specific_culture, english_name, local_name) VALUES (@Code0, @SpecificCulture0, @EnglishName0, @LocalName0) RETURNING *;", sql);

        }

        [TestMethod]
        public void ShouldReturnInsertStatementForManyEntities() {
            var provider = new PgSyntaxProvider<Culture>(new CultureMetadata(), log.Object);

            var culture = new Culture { Code = "Test", EnglishName = "Test", LocalName = "Test", SpecificCulture = "Test" };
            var culture1 = new Culture { Code = "Test1", EnglishName = "Test1", LocalName = "Test1", SpecificCulture = "Test1" };

            // Assert
            var sql = provider.Insert(new[] { culture, culture1 }, out var parameters);

            // Act
            var keyEnum = parameters.Keys.GetEnumerator();
            var valueEnum = parameters.Values.GetEnumerator();
            Assert.IsTrue(keyEnum.MoveNext());
            Assert.IsTrue(valueEnum.MoveNext());

            Assert.AreEqual(8, parameters.Count);
            Assert.AreEqual("Code0", keyEnum.Current);
            Assert.AreEqual("Test", valueEnum.Current);
            Assert.AreEqual("INSERT INTO app.culture (code, specific_culture, english_name, local_name) VALUES (@Code0, @SpecificCulture0, @EnglishName0, @LocalName0), (@Code1, @SpecificCulture1, @EnglishName1, @LocalName1) RETURNING *;", sql);

        }

    }
}