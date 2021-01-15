namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{
    /// <summary>
    /// Represents a AD user
    /// </summary>
    public class AdComputer : IAdObject
    {
        /// <summary>
        /// the distinguished name of the user
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// LDAP path to the user
        /// </summary>
        public string Path { get; set; }

        public string Name { get; set; }


    }
}