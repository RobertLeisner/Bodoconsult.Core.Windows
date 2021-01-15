using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{
    /// <summary>
    /// AD domain
    /// </summary>
    public class AdDomain
    {

        private readonly object _locker = new object();

        /// <summary>
        /// Default ctor
        /// </summary>
        public AdDomain(string path)
        {
            Path = path;
            DhcpServers = new List<AdComputer>();
            Computers = new List<AdComputer>();
            GeneralInfos = new Dictionary<string, string>();
            Users = new List<AdUser>();
            Groups = new List<AdGroup>();
            GetGeneralInfos();
        }

        /// <summary>
        /// General info on AD structure
        /// </summary>
        public Dictionary<string, string> GeneralInfos { get; set; }


        /// <summary>
        /// LDAP path to the domain
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Name of the domain
        /// </summary>
        public string FqdnName { get; set; }



        /// <summary>
        /// Domain users
        /// </summary>
        public IList<AdUser> Users { get; set; }


        /// <summary>
        /// AD group
        /// </summary>
        public IList<AdGroup> Groups { get; set; }


        /// <summary>
        /// DHCP servers
        /// </summary>
        public List<AdComputer> DhcpServers { get; set; }


        /// <summary>
        /// All computers in the domain
        /// </summary>
        public List<AdComputer> Computers { get; set; }




        private void GetGeneralInfos()
        {
            var deRoot = new DirectoryEntry("LDAP://RootDSE");


            // AG general data
            if (deRoot != null)
            {

                foreach (string propName in deRoot.Properties.PropertyNames)
                {
                    var x = deRoot.Properties[propName].Value.GetType().Name;
                    if (x == "Object[]")
                    {
                        var s = "";

                        foreach (var data in (object[])deRoot.Properties[propName].Value)
                        {
                            s += data + "\r\n";
                        }

                        GeneralInfos.Add(propName, s);
                    }
                    else
                    {
                        GeneralInfos.Add(propName, deRoot.Properties[propName].Value.ToString());
                    }

                }
            }


            // DNS server



            // DHCP server

            var dhcpPath = "LDAP://CN=NetServices,CN=Services,CN=Configuration," + Path.Replace("LDAP://", "");

            var di = new DirectoryEntry(dhcpPath);

            using (var ds = new DirectorySearcher(di))
            {
                //ds.SearchScope = SearchScope.OneLevel;
                ds.PropertiesToLoad.Add("cn");
                ds.PropertiesToLoad.Add("distinguishedName");
                //ds.PropertiesToLoad.Add("userprincipalname");



                ds.Filter = "(ObjectClass=dHCPClass)";

                var results = ds.FindAll();

                foreach (SearchResult result in results)
                {

                    var computer = AddComputer(result.Path);
                    computer.Name = result.Properties["cn"][0].ToString();
                    computer.DistinguishedName = result.Properties["distinguishedName"][0].ToString();

                    //if (result.Properties["userprincipalname"].Count > 0)
                    //    user.UserPrincipalName = result.Properties["userprincipalname"][0].ToString();

                    //Debug.Print("");
                    //foreach (var p in result.Properties.PropertyNames)
                    //{
                    //    Debug.Print(p + ": " + result.Properties[p.ToString()][0].ToString());
                    //}

                    DhcpServers.Add(computer);
                    Computers.Add(computer);
                }
            }

        }

        /// <summary>
        /// Delivers a new computer or a existing computer for the given LDAP path
        /// </summary>
        /// <param name="path">LDAP path</param>
        /// <returns>Computer item</returns>
        public AdComputer AddComputer(string path)
        {

            lock (_locker)
            {
                var computer = Computers.FirstOrDefault(x => x.Path == path);
                if (computer != null) return computer;

                computer = new AdComputer { Path = path };
                Computers.Add(computer);
                return computer;
            }

        }

        /// <summary>
        /// Delivers a new user or a existing user for the given LDAP path
        /// </summary>
        /// <param name="path">LDAP path</param>
        /// <returns>Computer item</returns>
        public AdUser AddUser(string path)
        {

            lock (_locker)
            {

                var user = Users.FirstOrDefault(x => x.Path == path);
                if (user != null) return user;
                user = new AdUser { Path = path };

                Users.Add(user);
                return user;
            }


        }


        /// <summary>
        /// Delivers a new user or a existing user for the given LDAP path
        /// </summary>
        /// <param name="path">LDAP path</param>
        /// <returns>Computer item</returns>
        public AdGroup AddGroup(string path)
        {

            lock (_locker)
            {

                var group = Groups.FirstOrDefault(x => x.Path == path);
                if (group != null) return group;
                group = new AdGroup { Path = path };
                Groups.Add(group);
                return group;
            }
        }

    }

}
