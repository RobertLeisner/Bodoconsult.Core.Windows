using System.IO;
using Bodoconsult.Core.Windows.FileSystem;
using Bodoconsult.Core.Windows.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Windows.Test
{
    [TestFixture]
    public class UnitTestFileSystemUrl
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void TestCtor()
        {

            // Arrange
            var url = Path.Combine(TestHelper.TestDataPath, "Bodoconsult.url");

            var fri = new FileInfo(url);

            // Act
            var urlFile = new FileSystemUrl(fri);

            // Assert
            Assert.AreEqual("Bodoconsult", urlFile.Caption);
        }

        [Test]
        public void TestRead()
        {

            // Arrange
            var url = Path.Combine(TestHelper.TestDataPath, "Bodoconsult.url");

            var fri = new FileInfo(url);

            var urlFile = new FileSystemUrl(fri);

            // Act
            urlFile.Read();

            // Assert
            Assert.AreEqual("http://www.bodoconsult.de/", urlFile.Url);
            Assert.AreEqual("Bodoconsult", urlFile.Caption);
        }
    }
}