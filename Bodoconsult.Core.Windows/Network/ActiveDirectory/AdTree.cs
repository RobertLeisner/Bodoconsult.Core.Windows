// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{

    /// <summary>
    /// Represents a AD tree
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class AdTree
    {
        public DirectoryEntry RootOu { get; set; }

        /// <summary>
        /// AD Domain
        /// </summary>
        public AdDomain Domain { get; set; }


        public string RootDn { get; set; }


        public string Name { get; set; }

        /// <summary>
        /// All child OUs in the OU
        /// </summary>
        public List<AdTree> ChildOUs { get; set; } = new List<AdTree>();

        /// <summary>
        /// All users in the OU
        /// </summary>
        public List<AdUser> Users { get; set; }


        /// <summary>
        /// All users in the OU
        /// </summary>
        public List<AdGroup> Groups { get; set; }







        /// <summary>
        /// All computers in the OU
        /// </summary>
        public List<AdComputer> Computers { get; set; }

        /// <summary>
        /// Add a tree by distinguished name
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="showUserAndComputers">Show users and cmputers for current node. Default: true</param>
        public AdTree(string dn, bool showUserAndComputers = true)
        {

            var domainName = dn.Replace("LDAP://DC=", "").Replace("DC=", ".").Replace(",","");

            Domain = new AdDomain(dn)
            {
                FqdnName = domainName,
            };

            Users = new List<AdUser>();
            Groups = new List<AdGroup>();
            Computers = new List<AdComputer>();

            RootOu = new DirectoryEntry(dn);
            RootDn = dn;
            Name = RootOu.Name.Replace("DC=", "").Replace("Dc=", "").Replace("dc=", "").Replace("OU=", "");


            if (showUserAndComputers)
            {
                AddUsers();

                AddGroups();

                AddComputers();
            }

            BuildAdTree().Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="domain"></param>
        public AdTree(DirectoryEntry root, AdDomain domain)
        {
            Domain = domain;
            Users = new List<AdUser>();
            Groups = new List<AdGroup>();
            Computers = new List<AdComputer>();


            RootOu = root;
            RootDn = root.Path;
            Name = RootOu.Name.Replace("DC=", "").Replace("Dc=", "").Replace("dc=", "").Replace("OU=", "");


            AddUsers();

            AddGroups();

            AddComputers();

            BuildAdTree().Wait();
        }




        /// <summary>
        /// Add users to <see cref="AdTree"/>
        /// </summary>
        private void AddUsers()
        {
            using (var ds = new DirectorySearcher(RootOu))
            {
                ds.SearchScope = SearchScope.OneLevel;
                ds.PropertiesToLoad.Add("cn");
                ds.PropertiesToLoad.Add("userprincipalname");
                ds.PropertiesToLoad.Add("memberOf");


                ds.Filter = "(&(objectClass=user)(objectCategory=person))";

                var results = ds.FindAll();

                foreach (SearchResult result in results)
                {



                    var user = Domain.AddUser(result.Path);

                    user.Name = result.Properties["cn"][0].ToString();

                    if (result.Properties["userprincipalname"].Count > 0)
                        user.UserPrincipalName = result.Properties["userprincipalname"][0].ToString();


                    if (result.Properties["memberOf"].Count > 0)
                    {

                        foreach (var o in result.Properties["memberOf"])
                        {
                            var path = "LDAP://" + o;
                            var entry = new DirectoryEntry(path);


                            var group = Domain.AddGroup(path);
                            group.Name= entry.Properties["cn"][0].ToString();

                            user.MemberOf.Add(group);
                        }

                    }

                    //Debug.Print("");
                    //foreach (var p in result.Properties.PropertyNames)
                    //{
                    //    Debug.Print(p + ": " + result.Properties[p.ToString()][0].ToString());
                    //}

                    Users.Add(user);
                }
            }

        }

        /// <summary>
        /// Add groups to <see cref="AdTree"/>
        /// </summary>
        private void AddGroups()
        {
            using (var ds = new DirectorySearcher(RootOu))
            {
                ds.SearchScope = SearchScope.OneLevel;
                ds.PropertiesToLoad.Add("cn");
                ds.PropertiesToLoad.Add("distinguishedName");
                ds.PropertiesToLoad.Add("groupType");


                ds.Filter = "(objectCategory=group)";

                var results = ds.FindAll();

                foreach (SearchResult result in results)
                {

                    var group = Domain.AddGroup(result.Path);
                    group.Name = result.Properties["cn"][0].ToString();
                    group.DistinguishedName = result.Properties["distinguishedName"][0].ToString();
                    group.GroupType = (AdGroupType)Convert.ToInt32(result.Properties["groupType"][0].ToString());

                    // find all users in this group
                    var dsm = new DirectorySearcher(Domain.Path)
                    {
                        Filter = $"(&(memberOf={group.DistinguishedName})(objectCategory=person))"    //(objectClass=person)
                    };
                    dsm.PropertiesToLoad.Add("cn");

                    foreach (SearchResult sr in dsm.FindAll())
                    {
                        var member = Domain.AddUser(sr.Path);
                        member.Name = sr.Properties["cn"][0].ToString();
                        group.Members.Add(member);
                    }

                    // find all groups in this group
                    dsm = new DirectorySearcher(Domain.Path)
                    {
                        Filter = $"(&(memberOf={group.DistinguishedName})(objectCategory=group))"    //(objectClass=person)
                    };
                    dsm.PropertiesToLoad.Add("cn");
                    dsm.PropertiesToLoad.Add("groupType");


                    foreach (SearchResult sr in dsm.FindAll())
                    {
                        var member = Domain.AddGroup(sr.Path);
                        member.Name = sr.Properties["cn"][0].ToString();
                        member.GroupType = (AdGroupType) Convert.ToInt32(sr.Properties["groupType"][0].ToString());
                        group.Members.Add(member);
                    }


                    //if (result.Properties["userprincipalname"].Count > 0)
                    //    user.UserPrincipalName = result.Properties["userprincipalname"][0].ToString();

                    //Debug.Print("");
                    //foreach (var p in result.Properties.PropertyNames)
                    //{
                    //    Debug.Print(p + ": " + result.Properties[p.ToString()][0].ToString());
                    //}

                    Groups.Add(group);
                }
            }

        }

        /// <summary>
        /// Add computers to <see cref="AdTree"/>
        /// </summary>
        private void AddComputers()
        {
            using (var ds = new DirectorySearcher(RootOu))
            {
                ds.SearchScope = SearchScope.OneLevel;
                ds.PropertiesToLoad.Add("cn");
                //ds.PropertiesToLoad.Add("userprincipalname");



                ds.Filter = "(objectClass=computer)";

                var results = ds.FindAll();

                foreach (SearchResult result in results)
                {

                    var computer = Domain.AddComputer(result.Path);
                    computer.Name = result.Properties["cn"][0].ToString();

                    //if (result.Properties["userprincipalname"].Count > 0)
                    //    user.UserPrincipalName = result.Properties["userprincipalname"][0].ToString();

                    //Debug.Print("");
                    //foreach (var p in result.Properties.PropertyNames)
                    //{
                    //    Debug.Print(p + ": " + result.Properties[p.ToString()][0].ToString());
                    //}

                    Computers.Add(computer);
                }
            }

        }

        private Task BuildAdTree()
        {
            return Task.Factory.StartNew(() =>
            {
                var locker = new object();
                Parallel.ForEach(RootOu.Children.Cast<DirectoryEntry>().AsEnumerable(), child =>
                {
                    var childTree = new AdTree(child, Domain);
                    if (!child.SchemaClassName.Equals("organizationalUnit")) return;
                    lock (locker)
                    {
                        ChildOUs.Add(childTree);
                    }
                });
            });
        }
    }
}
