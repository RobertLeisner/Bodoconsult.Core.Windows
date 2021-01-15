namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerHost
    {
        DhcpServerIpAddress Address { get; }
        string NetBiosName { get; }
        string ServerName { get; }
    }
}
