namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{
    /// <summary>
    /// General interface of AD objects
    /// </summary>
    public interface IAdObject
    {
        /// <summary>
        /// LDAP path to the object
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Name of the object
        /// </summary>
        string Name { get; set; }

    }
}
