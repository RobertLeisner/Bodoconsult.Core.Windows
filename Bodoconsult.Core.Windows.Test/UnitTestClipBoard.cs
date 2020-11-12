using Bodoconsult.Core.Windows.System;
using NUnit.Framework;

namespace Bodoconsult.Core.Windows.Test
{
    [TestFixture]
    public class UnitTestClipBoard
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void TestSetText()
        {

            // Arrange
            var text = "CopyToClipboard";

            // Act
            Clipboard.SetText(text);

            var result = Clipboard.GetText();


            // Assert
            Assert.AreEqual(text, result);
        }
    }
}