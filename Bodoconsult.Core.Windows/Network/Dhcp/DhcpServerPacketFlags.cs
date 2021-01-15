using System;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    [Flags]
    public enum DhcpServerPacketFlags : ushort
    {
        Broadcast = 0x8000
    }
}
