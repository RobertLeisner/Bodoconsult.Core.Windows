using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerFailoverRelationshipCollection : IEnumerable<IDhcpServerFailoverRelationship>
    {
        IDhcpServer Server { get; }

        IDhcpServerFailoverRelationship GetRelationship(string relationshipName);
        void RemoveRelationship(IDhcpServerFailoverRelationship relationship);
    }
}
