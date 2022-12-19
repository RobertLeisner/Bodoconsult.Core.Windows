# What does the library

Bodoconsult.Core.Windows provides features related to Microsoft Windows operating system.

Current features are:

+Icon extraction as bitmap
+Reading data from url files (get the included link address in file)


# How to use the library

The source code contain a NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Using FileSystemUrl classes: extract a link address

            // Arrange
            var url = Path.Combine(TestHelper.TestDataPath, "Bodoconsult.url");

            var fri = new FileInfo(url);

            var urlFile = new FileSystemUrl(fri);

            // Act
            urlFile.Read();

            // Assert
            Assert.AreEqual("http://www.bodoconsult.de/", urlFile.Url);
            Assert.AreEqual("Bodoconsult", urlFile.Caption);

## Using IconsAsFilesHelper to get GIF images from an app icon

            var iconDocx = Path.Combine(TestHelper.OutputPath, "docx.gif");
            if (File.Exists(iconDocx)) File.Delete(iconDocx);

            var iconXlsx = Path.Combine(TestHelper.OutputPath, "xlsx.gif");
            if (File.Exists(iconXlsx)) File.Delete(iconXlsx);


            var icons = new IconsAsFilesHelper {IconPath = TestHelper.OutputPath};

            var path = Path.Combine(TestHelper.TestDataPath, "Test.docx");

            var fri = new FileInfo(path);
            icons.AddExtension(fri);


            path = Path.Combine(TestHelper.TestDataPath, "Test.xlsx");

            fri = new FileInfo(path);
            icons.AddExtension(fri);

            icons.SaveIcons();

            Assert.IsTrue(File.Exists(iconDocx));
            Assert.IsTrue(File.Exists(iconXlsx));

## Meta data services

### DNS (via AD and CIM)


            var pwd = TestHelper.GetSecureString(Password);

            var d = new DnsServer(DomainServer, Domain, UserName, pwd);

            Debug.Print("DNS structure "+ settings.DomainServer);

            Debug.Print("DNS domains");
            foreach (var domain in d.GetListOfDomains())
            {
                Debug.Print("\t" + domain.Name + " (" + domain.ZoneType+ (domain.ReverseZone ? ", Reverse zone":"")+ ")");
                //and a list of all the records in the domain:-
                foreach (var record in d.GetRecordsForDomain(domain.Name))
                {
                    Debug.Print("\t\t" + record);
                    //any domains we are primary for we could go and edit the record now!
                }
            }

### DHCP

            var dhcpServer = DhcpServer.Connect(dhcpServerName);

            Result.AddHeader1("DHCP server " + dhcpServerName);
            Result.AddParagraph("Domain: "+_domain);
            Result.AddHeader2("Basic DHCP Configuration");

            // Display some configuration
            Result.AddDefinitionListLine("Protocol Support:", dhcpServer.Configuration.ApiProtocolSupport.ToString());
            Result.AddDefinitionListLine("Database Name:", dhcpServer.Configuration.DatabaseName);
            Result.AddDefinitionListLine("Database Path:", dhcpServer.Configuration.DatabasePath);

            // Show all bound interfaces
            foreach (var binding in dhcpServer.BindingElements)
            {
                Result.AddDefinitionListLine("Binding Interface Id:", binding.InterfaceGuidId.ToString());
                Result.AddDefinitionListLine("Description:", binding.InterfaceDescription);
                Result.AddDefinitionListLine("Adapter Address:", binding.AdapterPrimaryIpAddress.ToString());
                Result.AddDefinitionListLine("Adapter Subnet:", binding.AdapterSubnetAddress.ToString());
            }


            // Display scope information
            Result.AddHeader2("Scope information");
            foreach (var scope in dhcpServer.Scopes)
            {
                Result.AddHeader3($"Scope '{scope.Name}'");
                Result.AddDefinitionListLine("Address:", scope.Address.ToString());
                Result.AddDefinitionListLine("Mask:", scope.Mask.ToString());
                Result.AddDefinitionListLine("Range:", scope.IpRange.ToString());
                Result.AddDefinitionListLine("State:", scope.State.ToString());

                var activeClients = scope.Clients
                    .Where(c => c.AddressState == DhcpServerClientAddressStates.Active);

                Result.AddHeader4("Client leases");

                // Display client information
                foreach (var client in activeClients.OrderBy(x => x.IpAddress.ToString()))
                {
                    Result.AddDefinitionListLine(client.IpAddress.ToString(),
                        $"[{client.HardwareAddress}] {client.Name}, Expires: {client.LeaseExpires}");
                }

                Result.AddHeader4("Reservations");
                foreach (var reservation in scope.Reservations.OrderBy(x => x.IpAddress.ToString()))
                {
                    Result.AddDefinitionListLine(reservation.IpAddress.ToString(),
                        $"[{reservation.HardwareAddress}] {reservation.Client.Name}");
                }


                Result.AddHeader4("Scope options");
                foreach (var optionValue in scope.OptionValues)
                {
                    var s = "";
                    foreach (var value in optionValue.Values)
                    {
                        s += $"{value.Value} [{value.Type}]\r\n";
                    }

                    Result.AddDefinitionListLine($"{optionValue.Option.Name} [{optionValue.OptionId}]:", s);
                }
            }

### Active Directory: get general domain infos, dhcp servers and all users and groups

          public AdTree GetAdDomainInfos

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
			
			return root;
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



# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

