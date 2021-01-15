using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerClientCollection : IEnumerable<IDhcpServerClient>
    {
        IDhcpServer Server { get; }

        void RemoveClient(IDhcpServerClient client);
    }
}
