﻿using System;
using System.Runtime.InteropServices;

namespace Bodoconsult.Core.Windows.Network.Dhcp.Native
{
    /// <summary>
    /// The DHCP_IP_RESERVATION_V4 structure defines a client IP reservation. This structure extends an IP reservation by including the type of client (DHCP or BOOTP) holding the reservation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DHCP_IP_RESERVATION_V4 : IDisposable
    {
        /// <summary>
        /// DHCP_IP_ADDRESS value that contains the reserved IP address.
        /// </summary>
        public readonly DHCP_IP_ADDRESS ReservedIpAddress;
        /// <summary>
        /// DHCP_CLIENT_UID structure that contains the hardware address (MAC address) of the DHCPv4 client that holds this reservation.
        /// </summary>
        private readonly IntPtr ReservedForClientPointer;
        /// <summary>
        /// Value that specifies the DHCPv4 reserved client type.
        /// </summary>
        public readonly DHCP_CLIENT_TYPE bAllowedClientTypes;

        /// <summary>
        /// DHCP_CLIENT_UID structure that contains the hardware address (MAC address) of the DHCPv4 client that holds this reservation.
        /// </summary>
        public DHCP_CLIENT_UID ReservedForClient => ReservedForClientPointer.MarshalToStructure<DHCP_CLIENT_UID>();

        public void Dispose()
        {
            if (ReservedForClientPointer != IntPtr.Zero)
            {
                ReservedForClient.Dispose();
                Api.FreePointer(ReservedForClientPointer);
            }
        }
    }

    /// <summary>
    /// The DHCP_IP_RESERVATION_V4 structure defines a client IP reservation. This structure extends an IP reservation by including the type of client (DHCP or BOOTP) holding the reservation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DHCP_IP_RESERVATION_V4_Managed : IDisposable
    {
        /// <summary>
        /// DHCP_IP_ADDRESS value that contains the reserved IP address.
        /// </summary>
        public readonly DHCP_IP_ADDRESS ReservedIpAddress;
        /// <summary>
        /// DHCP_CLIENT_UID structure that contains the hardware address (MAC address) of the DHCPv4 client that holds this reservation.
        /// </summary>
        private readonly IntPtr ReservedForClientPointer;
        /// <summary>
        /// Value that specifies the DHCPv4 reserved client type.
        /// </summary>
        public readonly DHCP_CLIENT_TYPE AllowedClientTypes;

        public DHCP_IP_RESERVATION_V4_Managed(DHCP_IP_ADDRESS reservedIpAddress, DHCP_CLIENT_UID_Managed reservedForClient, DHCP_CLIENT_TYPE allowedClientTypes)
        {
            ReservedIpAddress = reservedIpAddress;
            AllowedClientTypes = allowedClientTypes;
            ReservedForClientPointer = Marshal.AllocHGlobal(Marshal.SizeOf(reservedForClient));
            Marshal.StructureToPtr(reservedForClient, ReservedForClientPointer, false);
        }

        public DHCP_IP_RESERVATION_V4_Managed(DhcpServerIpAddress address, DhcpServerHardwareAddress reservedForClient, DhcpServerClientTypes allowedClientTypes)
            :this(reservedIpAddress: address.ToNativeAsNetwork(),
                  reservedForClient: reservedForClient.ToNativeClientUid(),
                  allowedClientTypes: (DHCP_CLIENT_TYPE)allowedClientTypes)
        { }

        public void Dispose()
        {
            if (ReservedForClientPointer != IntPtr.Zero)
            {
                var reservedForClient = ReservedForClientPointer.MarshalToStructure<DHCP_CLIENT_UID_Managed>();
                reservedForClient.Dispose();

                Marshal.FreeHGlobal(ReservedForClientPointer);
            }
        }
    }
}
