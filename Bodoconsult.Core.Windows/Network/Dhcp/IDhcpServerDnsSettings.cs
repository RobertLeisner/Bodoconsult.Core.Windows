namespace Bodoconsult.Core.Windows.Network.Dhcp
{
    public interface IDhcpServerDnsSettings
    {
        bool DisableDynamicPtrRecordUpdates { get; }
        bool DiscardRecordsWhenLeasesDeleted { get; }
        bool DynamicDnsUpdatedAlways { get; }
        bool DynamicDnsUpdatedOnlyWhenRequested { get; }
        bool DynamicDnsUpdatesEnabled { get; }
        bool UpdateRecordsForDownLevelClients { get; }
    }
}
