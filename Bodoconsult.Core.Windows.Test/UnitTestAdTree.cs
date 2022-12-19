// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Bodoconsult.Core.Windows.Network.ActiveDirectory;
using Bodoconsult.Core.Windows.Network.ActiveDirectory.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Windows.Test
{
    [TestFixture]
    [SupportedOSPlatform("windows")]
    public class UnitTestAdTree
    {
        [Test]
        public void TestCtor()
        {

            AdTree root = null;

            var buildOuStructure = Task.Factory.StartNew(() => { root = new AdTree(AdHelper.GetLdapDomainForCurrentUser(), false); });

            buildOuStructure.Wait();

            var domain = root.Domain;

            // Get user data
            AdHelper.GetUserData(domain);

            Assert.IsTrue(root != null);

            Debug.Print("General AD info");
            foreach (var prop in domain.GeneralInfos)
            {
                Debug.Print(prop.Key +": " +prop.Value);
            }


            Debug.Print("\r\n\r\nDHCP");
            foreach (var dhcp in domain.DhcpServers)
            {
                Debug.Print(dhcp.Name );
                Debug.Print(dhcp.DistinguishedName);
            }


            EnumerateTree(root, 0);
        }

        //(objectCategory=group)

        private void EnumerateTree(AdTree node, int level)
        {

            var space = "".PadRight(level * 5);
            var space1 = "".PadRight((level + 1) * 5);
            var space2 = "".PadRight((level + 2) * 5);

            Debug.Print("");
            Debug.Print("");
            Debug.Print(space + node.Name);



            if (node.Computers.Any())
            {
                Debug.Print(space1 + "Computers");

                foreach (var computer in node.Computers)
                {
                    Debug.Print(space2 + "Computer: " + computer.Name);
                }
            }

            if (node.Users.Any())
            {
                Debug.Print(space1 + "Users");

                foreach (var user in node.Users)
                {
                    Debug.Print(space2 + "User: " + user.Name);

                    foreach (var m in user.MemberOf)
                    {
                        Debug.Print(space2 + "  "+m.Name);
                    }
                }
            }


            if (node.Groups.Any())
            {
                Debug.Print(space1 + "Groups");

                foreach (var group in node.Groups)
                {
                    Debug.Print(space2 + "Group: " + group.Name + " ("+(AdGroupTypeShort)group.GroupType+")");

                    foreach (var m in group.Members)
                    {
                        Debug.Print(space2 + "  " + m.Name);
                    }

                }
            }

            if (node.ChildOUs.Any())
            {
                Debug.Print(space1 + "OUs");
                foreach (var subNode in node.ChildOUs.OrderBy(x => x.Name))
                {
                    EnumerateTree(subNode, level + 2);
                }
            }
        }
    }
}
