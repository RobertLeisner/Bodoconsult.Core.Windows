using System.Diagnostics;
using Bodoconsult.Core.Windows.Network.ActiveDirectory.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Windows.Test
{
    [TestFixture]
    public class UnitTestAdHelper
    {
        [Test]
        public void TestGetLdapDomainForCurrentUser()
        {
            var result = AdHelper.GetLdapDomainForCurrentUser();

            Debug.Print(result);

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

    }
}