﻿using System;
using System.Runtime.InteropServices;

namespace Bodoconsult.Core.Windows.Network.Dhcp.Native
{
    /// <summary>
    /// The DHCP_SERVER_SPECIFIC_STRINGS structure contains the default string values for user and vendor class names.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DHCP_SERVER_SPECIFIC_STRINGS : IDisposable
    {
        /// <summary>
        /// Pointer to a Unicode string that specifies the default vendor class name for the DHCP server.
        /// </summary>
        private readonly IntPtr DefaultVendorClassNamePointer;
        /// <summary>
        /// Pointer to a Unicode string that specifies the default user class name for the DHCP server.
        /// </summary>
        private readonly IntPtr DefaultUserClassNamePointer;

        /// <summary>
        /// Pointer to a Unicode string that specifies the default vendor class name for the DHCP server.
        /// </summary>
        public string DefaultVendorClassName => Marshal.PtrToStringUni(DefaultVendorClassNamePointer);
        /// <summary>
        /// Pointer to a Unicode string that specifies the default user class name for the DHCP server.
        /// </summary>
        public string DefaultUserClassName => Marshal.PtrToStringUni(DefaultUserClassNamePointer);

        public void Dispose()
        {
            Api.FreePointer(DefaultVendorClassNamePointer);
            Api.FreePointer(DefaultUserClassNamePointer);
        }
    }
}
