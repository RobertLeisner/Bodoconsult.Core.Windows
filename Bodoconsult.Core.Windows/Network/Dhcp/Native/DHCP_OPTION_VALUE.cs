﻿using System;
using System.Runtime.InteropServices;

namespace Bodoconsult.Core.Windows.Network.Dhcp.Native
{
    /// <summary>
    /// The DHCP_OPTION_VALUE structure defines a DHCP option value (just the option data with an associated ID tag).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DHCP_OPTION_VALUE : IDisposable
    {
        /// <summary>
        /// DHCP_OPTION_ID value that specifies a unique ID number for the option.
        /// </summary>
        public readonly int OptionID;
        /// <summary>
        /// DHCP_OPTION_DATA structure that contains the data for a DHCP server option.
        /// </summary>
        public readonly DHCP_OPTION_DATA Value;

        public void Dispose()
        {
            Value.Dispose();
        }
    }
}
