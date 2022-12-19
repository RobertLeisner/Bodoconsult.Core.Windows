// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;

namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{
    /// <summary>
    /// General interface of AD objects
    /// </summary>
    [SupportedOSPlatform("windows")]
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
