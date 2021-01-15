namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerAuditLog
    {
        string AuditLogDirectory { get; }
        int DiskCheckInterval { get; }
        int MaxLogFilesSize { get; }
        int MinSpaceOnDisk { get; }
        IDhcpServer Server { get; }
    }
}
