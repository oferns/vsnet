﻿namespace VS.Core.Tests {
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VS.Abstractions;
    using VS.Core.Local.Storage;

    [TestClass]
    public class StorageTests {

        [TestClass]
        public class FileStorageTests {

            [TestClass]
            public class CTor {

                [TestMethod]
                // Assert
                [ExpectedException(typeof(ArgumentNullException))]
                public void ShouldThrowOnNullFileProvider() {
                    // Arrange & Act
                    _ = new FileStorageClient(null, null, null);
                }

                [TestMethod]
                // Assert
                [ExpectedException(typeof(ArgumentNullException))]
                public void ShouldThrowOnNullContentTypeProvider() {
                    // Arrange & Act
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    _ = new FileStorageClient(mockFileProvider.Object, null, null);
                }

                [TestMethod]
                // Assert
                [ExpectedException(typeof(ArgumentNullException))]
                public void ShouldThrowOnNullSerializer() {
                    // Arrange & Act
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    _ = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, null);
                }


            }

            [TestClass]
            public class Exists {
                [TestMethod]
                public async Task ShouldReturnTrueWhenAbsoluteUriExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.Exists).Returns(true).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act
                    var exists = await client.Exists(new Uri("file://test"), CancellationToken.None);

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsTrue(exists);
                }

                [TestMethod]
                public async Task ShouldReturnFalseWhenAbsoluteUriDoesNotExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.Exists).Returns(false).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act
                    var exists = await client.Exists(new Uri("file://test"), CancellationToken.None);
                    
                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsFalse(exists);
                }

                [TestMethod]
                public void ShouldReturnTrueWhenRelativeUriExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.Exists).Returns(true).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act
                    var exists = client.Exists(new Uri("/test", UriKind.Relative), CancellationToken.None).Result;

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsTrue(exists);
                }

                [TestMethod]
                public void ShouldReturnFalseWhenRelativeUriDoesNotExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.Exists).Returns(false).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act
                    var exists = client.Exists(new Uri("/test", UriKind.Relative), CancellationToken.None).Result;

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsFalse(exists);
                }
            }

            [TestClass]
            public class Get {

                [TestMethod]
                public async Task ShouldReturnStreamWhenAbsoluteUrlExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.CreateReadStream()).Returns(new MemoryStream()).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act                    
                    using var filestream = await client.Get(new Uri("file://test"), CancellationToken.None);

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsNotNull(filestream);
                }

                [TestMethod]
                public async Task ShouldReturnDefaultStreamWhenAbsoluteUrlDoesNotExist() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.CreateReadStream()).Returns(default(MemoryStream)).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act                    
                    using var filestream = await client.Get(new Uri("file://test"), CancellationToken.None);
                    
                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsNull(filestream);
                }

                [TestMethod]
                public async Task ShouldReturnStreamWhenRelativeUrlExists() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.CreateReadStream()).Returns(new MemoryStream()).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act                    
                    using var filestream = await client.Get(new Uri("/test", UriKind.Relative), CancellationToken.None);

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsNotNull(filestream);
                }

                [TestMethod]
                public async Task ShouldReturnDefaultStreamWhenRelativeUrlDoesNotExist() {
                    // Arrange
                    var mockFileProvider = new Mock<IFileProvider>(MockBehavior.Strict);
                    var mockInfo = new Mock<IFileInfo>(MockBehavior.Strict);
                    var mockContentTypeProvider = new Mock<IContentTypeProvider>(MockBehavior.Strict);
                    var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);

                    mockFileProvider.Setup(m => m.GetFileInfo(It.IsAny<string>())).Returns(mockInfo.Object).Verifiable();
                    mockInfo.Setup(m => m.CreateReadStream()).Returns(default(MemoryStream)).Verifiable();

                    var client = new FileStorageClient(mockFileProvider.Object, mockContentTypeProvider.Object, mockSerializer.Object);

                    // Act                    
                    using var filestream = await client.Get(new Uri("/test", UriKind.Relative), CancellationToken.None);

                    // Assert
                    mockFileProvider.Verify();
                    mockInfo.Verify();
                    Assert.IsNull(filestream);
                }

            }
        }
    }
}