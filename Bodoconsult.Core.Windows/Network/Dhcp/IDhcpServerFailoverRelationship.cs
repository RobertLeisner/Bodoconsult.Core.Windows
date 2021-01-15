﻿using System;
using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerFailoverRelationship
    {
        byte? HotStandbyAddressesReservedPercentage { get; }
        byte? LoadBalancePercentage { get; }
        TimeSpan MaximumClientLeadTime { get; }
        DhcpServerFailoverMode Mode { get; }
        string Name { get; }
        DhcpServerFailoverState PreviousState { get; }
        DhcpServerIpAddress PrimaryServerAddress { get; }
        string PrimaryServerName { get; }
        IEnumerable<IDhcpServerScope> Scopes { get; }
        DhcpServerIpAddress SecondaryServerAddress { get; }
        string SecondaryServerName { get; }
        IDhcpServer Server { get; }
        DhcpServerFailoverServerType ServerType { get; }
        string SharedSecret { get; }
        DhcpServerFailoverState State { get; }
        TimeSpan? StateSwitchoverInterval { get; }

        IDhcpServer ConnectToPartner();
        void Delete();
        void RedistributesFreeAddresses();
        void ReplicateRelationship();
    }
}
