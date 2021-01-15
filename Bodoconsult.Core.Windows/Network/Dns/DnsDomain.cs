using System;
using System.Collections.Generic;
using Microsoft.Management.Infrastructure;

namespace Bodoconsult.Core.Windows.Network.Dns
{
    /// <summary>
    /// A zone in MS DNS Server
    /// </summary>
    public class DnsDomain
    {
        /// <summary>
        /// Create a DNS zone
        /// </summary>
        /// <param name="item">The object that represents it in MS DNS server</param>
        /// <param name="server">The DNS Server it is to be managed by</param>
        public DnsDomain(CimInstance item, DnsServer server)
        {
            Name = item.CimInstanceProperties["ContainerName"].Value.ToString();
            _server = server;

            //Debug.Print("Reverse: "+item.CimInstanceProperties["Reverse"].Value.ToString());

            ReverseZone = (bool)item.CimInstanceProperties["Reverse"].Value;
            var zt = Convert.ToInt32(item.CimInstanceProperties["ZoneType"].Value.ToString());
            ZoneType = (ZoneType)zt;
        }

        private readonly DnsServer _server;

        /// <summary>
        /// The name of the DNS zone
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// The zone type
        /// </summary>
        public ZoneType ZoneType { get; }

        /// <summary>
        /// Is this a reverse DNS zone?
        /// </summary>
        public bool ReverseZone { get; }


        /// <summary>
        /// Get a list of all objects at the base of this zone
        /// </summary>
        /// <returns>A list of <see cref="DnsRecord"/></returns>
        public IList<DnsRecord> GetAllRecords()
        {
            return _server.GetRecordsForDomain(Name);
        }


        public override string ToString()
        {
            return Name;
        }
    }
}