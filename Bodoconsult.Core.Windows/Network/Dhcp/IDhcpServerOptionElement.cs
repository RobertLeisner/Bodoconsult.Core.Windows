using System;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerOptionElement : IEquatable<IDhcpServerOptionElement>
    {
        DhcpServerOptionElementType Type { get; }
        object Value { get; }
        string ValueFormatted { get; }
    }
}
