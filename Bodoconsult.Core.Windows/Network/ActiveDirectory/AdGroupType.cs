namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{

    /// <summary>
    /// Typ of AD group
    /// </summary>
    public enum AdGroupType
    {
        GlobalDistributionGroup = 2,
        DomainLocalDistributionGroup = 4,
        UniversalDistributionGroup = 8,
        GlobalSecurityGroup = -2147483646,
        DomainLocalSecurityGroup = -2147483644,
        UniversalSecurityGroup = -2147483640,
        BuiltInSecurityGroup = -2147483643
    }
}
