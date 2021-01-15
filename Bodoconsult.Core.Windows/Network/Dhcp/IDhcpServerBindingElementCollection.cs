using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerBindingElementCollection : IEnumerable<IDhcpServerBindingElement>
    {
        IDhcpServer Server { get; }
    }
}
