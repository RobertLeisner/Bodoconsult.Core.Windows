using System.IO;
using Bodoconsult.Core.Windows.Test.Helpers;
using Bodoconsult.Core.Windows.Test.Model;
using NUnit.Framework;

namespace Bodoconsult.Core.Windows.Test
{
    [TestFixture]
    public class UnitTestBasicSettings
    {
        [Test]
        public void TestGenerateAppSettings()
        {

            var fileName = Path.Combine(TestHelper.OutputPath, TestHelper.NameAppSettingsFile);

            if (File.Exists(fileName)) File.Delete(fileName);

            FileAssert.DoesNotExist(fileName);

            var s = new AppSettings
            {
                Domain = "xyz.de", 
                UserName = PasswordHandler.Encrypt("YourUserName"), 
                DomainServer = "FqnDnsServer",
                Password = PasswordHandler.Encrypt("YourPassword")
            };
           

            JsonHelper.SaveAsFile(fileName, s);


            FileAssert.Exists(fileName);

        }

    }
}