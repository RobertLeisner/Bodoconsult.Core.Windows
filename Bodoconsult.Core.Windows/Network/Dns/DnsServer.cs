using System.Collections.Generic;
using System.Security;
using Bodoconsult.Core.Windows.Network.Cim;
using Microsoft.Management.Infrastructure;

namespace Bodoconsult.Core.Windows.Network.Dns
{
    /// <summary>
    /// A Microsoft (r) DNS Server class that abstracts out calls to WMI for Microsoft (r) DNS Server
    /// </summary>
    /// <remarks>
    /// WMI Documentation: 
    /// http://msdn.microsoft.com/en-us/library/ms682123(VS.85).aspx
    /// System.Management Documentation: 
    /// http://msdn.microsoft.com/en-us/library/system.management.managementobjectcollection%28VS.71%29.aspx
    /// </remarks>
    /// <c> Based on http://www.nullify.net/Article/304 by Simon Soanes.
    ///
    /// (c) 2008 Simon Soanes, All Rights Reserved.  No warranties express or implied.
    /// DO NOT redistribute this source code publically without a link to the origin and this copyright
    /// notice fully intact, also please send any modifications back to me at simon@nullifynetwork.com
    /// Including in your software is fine although attribution would be nice. </c>
    /// 
    public class DnsServer
    {

        public string WmiPath { get; private set; }

        private CimSession _cimSession;


        /// <summary>
        /// Create a new DNS Server connection
        /// </summary>
        /// <param name="server">The hostname, IP or FQDN of a DNS server you have access to with the current credentials</param>
        public DnsServer(string server)
        {
            WmiPath = $@"\\{server}\Root\MicrosoftDNS";
            _cimSession = WmiCimHelper.GetSession(server);
        }

        /// <summary>
        /// Create a new DNS Server connection
        /// </summary>
        /// <param name="server">The hostname, IP or FQDN of a DNS server you have access to with the current credentials</param>
        /// <param name="domain">Domain name</param>
        /// <param name="username">The username to connect with</param>
        /// <param name="password">The users password</param>
        public DnsServer(string server, string domain, string username, SecureString password)
        {
            WmiPath = $@"\\{server}\Root\MicrosoftDNS"; 
            _cimSession = WmiCimHelper.GetSession(domain, server, username, password);

            //_cimSession = WmiCimHelper.GetSession("BCG-AD.DE", "BCGS02DC.BCG-AD.DE", "AdAdminRL", password);
        }

        /// <summary>
        /// The server this connection applies to
        /// </summary>
        public string Server { get; } = "";


        /// <summary>
        /// Return a list of domains managed by this instance of MS DNS Server
        /// </summary>
        /// <returns></returns>
        public IList<DnsDomain> GetListOfDomains()
        {

            var wql = "SELECT * FROM MicrosoftDNS_Zone";

            var allDomains = WmiCimHelper.ExecuteQuery(_cimSession, WmiPath, wql);


            var domains = new List<DnsDomain>();
            foreach (var item in allDomains)
            {
                domains.Add(new DnsDomain(item, this));
            }

            return domains.ToArray();
        }

        /// <summary>
        /// Return a list of records for a domain, note that it may include records
        /// that are stubs to other domains inside the zone and does not automatically
        /// recurse.
        /// </summary>
        /// <param name="domain">The domain to connect to</param>
        /// <returns></returns>
        public IList<DnsRecord> GetRecordsForDomain(string domain)
        {
            var wql = $"SELECT * FROM MicrosoftDNS_ResourceRecord WHERE DomainName='{domain}'";

            var allRecords = WmiCimHelper.ExecuteQuery(_cimSession, WmiPath, wql);

            var records = new List<DnsRecord>();
            foreach (var item in allRecords)
            {
                records.Add(new DnsRecord(item));
            }

            return records.ToArray();

            ;
        }


        ///// <summary>
        ///// Fetch DNS records for a particular name
        ///// WARNING: This method has performance issues, iterate over the results of getting all the records for a domain instead.
        ///// </summary>
        ///// <remarks>Returns a collection as one hostname/entry can have multiple records but it can take longer
        ///// than getting all the records and scanning them!</remarks>
        ///// <param name="hostName">The name to look up</param>
        ///// <returns></returns>
        //public DnsRecord[] GetExistingDnsRecords(string hostName)
        //{
        //    //var query = $"SELECT * FROM MicrosoftDNS_ResourceRecord WHERE OwnerName='{hostName}'";
        //    //var searcher = new ManagementObjectSearcher(_scope, new ObjectQuery(query));

        //    //var collection = searcher.Get();
        //    //var records = new List<DnsRecord>();
        //    //foreach (ManagementObject p in collection)
        //    //{
        //    //    records.Add(new DnsRecord(p));
        //    //}

        //    //return records.ToArray();

        //    return null;
        //}

        
        public override string ToString()
        {
            return Server;
        }
    }
}