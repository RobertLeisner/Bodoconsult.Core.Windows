using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerClassCollection : IEnumerable<IDhcpServerClass>
    {
        IDhcpServer Server { get; }

        IDhcpServerClass GetClass(string name);
    }
}
