using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerOptionValue
    {
        string ClassName { get; }
        IDhcpServerOption Option { get; }
        int OptionId { get; }
        IDhcpServer Server { get; }
        IEnumerable<IDhcpServerOptionElement> Values { get; }
        string VendorName { get; }
    }
}
