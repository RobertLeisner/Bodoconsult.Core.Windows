﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Bodoconsult.Core.Windows.Network.Dhcp.Native;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    /// <summary>
    /// Represents a DHCP Server
    /// </summary>
    public class DhcpServer : IDhcpServer
    {
        private IDhcpServerConfiguration configuration;
        private IDhcpServerAuditLog auditLog;
        private IDhcpServerDnsSettings dnsSettings;
        private IDhcpServerSpecificStrings specificStrings;

        public DhcpServerIpAddress Address { get; }
        public string Name { get; }
        public int VersionMajor { get; }
        public int VersionMinor { get; }
        public DhcpServerVersions Version => (DhcpServerVersions)(((ulong)VersionMajor << 16) | (uint)VersionMinor);

        public IDhcpServerConfiguration Configuration => configuration ??= DhcpServerConfiguration.GetConfiguration(this);
        /// <summary>
        /// The audit log configuration settings from the DHCP server.
        /// </summary>
        public IDhcpServerAuditLog AuditLog => auditLog ??= DhcpServerAuditLog.GetAuditLog(this);
        public IDhcpServerDnsSettings DnsSettings => dnsSettings ??= DhcpServerDnsSettings.GetGlobalDnsSettings(this);
        public IDhcpServerSpecificStrings SpecificStrings => specificStrings ??= DhcpServerSpecificStrings.GetSpecificStrings(this);

        private DhcpServer(DhcpServerIpAddress address, string name)
        {
            Address = address;
            Name = name;

            Classes = new DhcpServerClassCollection(this);
            Options = new DhcpServerOptionCollection(this);
            Scopes = new DhcpServerScopeCollection(this);
            Clients = new DhcpServerClientCollection(this);
            BindingElements = new DhcpServerBindingElementCollection(this);
            FailoverRelationships = new DhcpServerFailoverRelationshipCollection(this);

            GetVersion(address, out var versionMajor, out var versionMinor);
            VersionMajor = versionMajor;
            VersionMinor = versionMinor;
        }

        /// <summary>
        /// DHCP Server Classes
        /// </summary>
        public IDhcpServerClassCollection Classes { get; }

        /// <summary>
        /// DHCP Server Options
        /// </summary>
        public IDhcpServerOptionCollection Options { get; }

        /// <summary>
        /// DHCP Server Scopes
        /// </summary>
        public IDhcpServerScopeCollection Scopes { get; }

        /// <summary>
        /// Enumerates a list of all Clients (in all Scopes) associated with the DHCP Server
        /// </summary>
        public IDhcpServerClientCollection Clients { get; }

        /// <summary>
        /// Enumerates a list of Binding Elements associated with the DHCP Server
        /// </summary>
        public IDhcpServerBindingElementCollection BindingElements { get; }

        /// <summary>
        /// DHCP Server Failover Relationships
        /// </summary>
        public IDhcpServerFailoverRelationshipCollection FailoverRelationships { get; }

        /// <summary>
        /// Calculates if this server version is greater than or equal to the supplied <paramref name="version"/>
        /// </summary>
        /// <param name="version">Version to test compatibility against</param>
        /// <returns>True if the server version is greater than or equal to the supplied <paramref name="version"/></returns>
        public bool IsCompatible(DhcpServerVersions version) => ((long)version <= (long)Version);

        /// <summary>
        /// Enumerates a list of DHCP servers found in the directory service. 
        /// </summary>
        public static IEnumerable<IDhcpServer> Servers
        {
            get
            {
                var result = Api.DhcpEnumServers(Flags: 0,
                                                 IdInfo: IntPtr.Zero,
                                                 Servers: out var serversPtr,
                                                 CallbackFn: IntPtr.Zero,
                                                 CallbackData: IntPtr.Zero);

                if (result != DhcpErrors.SUCCESS)
                    throw new DhcpServerException(nameof(Api.DhcpEnumServers), result);

                try
                {
                    var servers = serversPtr.MarshalToStructure<DHCPDS_SERVERS>();

                    foreach (var server in servers.Servers)
                        yield return FromNative(in server);
                }
                finally
                {
                    Api.DhcpRpcFreeMemory(serversPtr);
                }
            }
        }

        /// <summary>
        /// Connects to a DHCP server
        /// </summary>
        /// <param name="hostNameOrAddress"></param>
        public static IDhcpServer Connect(string hostNameOrAddress)
        {
            if (string.IsNullOrWhiteSpace(hostNameOrAddress))
                throw new ArgumentNullException(nameof(hostNameOrAddress));

            var dnsEntry = global::System.Net.Dns.GetHostEntry(hostNameOrAddress);

            var dnsAddress = dnsEntry.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

            if (dnsAddress == null)
                throw new NotSupportedException("Unable to resolve an IPv4 address for the DHCP Server");

            var address = (DhcpServerIpAddress)dnsAddress;

            return new DhcpServer(address, dnsEntry.HostName);
        }

        private static void GetVersion(DhcpServerIpAddress address, out int versionMajor, out int versionMinor)
        {
            var result = Api.DhcpGetVersion(ServerIpAddress: address,
                                            MajorVersion: out versionMajor,
                                            MinorVersion: out versionMinor);

            if (result != DhcpErrors.SUCCESS)
                throw new DhcpServerException(nameof(Api.DhcpGetVersion), result);
        }

        private static DhcpServer FromNative(in DHCPDS_SERVER native)
            => new DhcpServer(native.ServerAddress.AsNetworkToIpAddress(), native.ServerName);

        public override string ToString() => $"{Name} ({Address})";

    }
}
