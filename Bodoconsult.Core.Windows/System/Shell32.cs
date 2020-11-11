using System;
using System.Runtime.InteropServices;

namespace Bodoconsult.Core.Windows.System
{
    public class Shell32
    {
        public const int MaxPath = 256;
        public const uint BIF_RETURNONLYFSDIRS = 1;
        public const uint BIF_DONTGOBELOWDOMAIN = 2;
        public const uint BIF_STATUSTEXT = 4;
        public const uint BIF_RETURNFSANCESTORS = 8;
        public const uint BIF_EDITBOX = 16;
        public const uint BIF_VALIDATE = 32;
        public const uint BIF_NEWDIALOGSTYLE = 64;
        public const uint BIF_USENEWUI = 80;
        public const uint BIF_BROWSEINCLUDEURLS = 128;
        public const uint BIF_BROWSEFORCOMPUTER = 4096;
        public const uint BIF_BROWSEFORPRINTER = 8192;
        public const uint BIF_BROWSEINCLUDEFILES = 16384;
        public const uint BIF_SHAREABLE = 32768;
        public const uint SHGFI_ICON = 256;
        public const uint SHGFI_DISPLAYNAME = 512;
        public const uint SHGFI_TYPENAME = 1024;
        public const uint SHGFI_ATTRIBUTES = 2048;
        public const uint SHGFI_ICONLOCATION = 4096;
        public const uint SHGFI_EXETYPE = 8192;
        public const uint SHGFI_SYSICONINDEX = 16384;
        public const uint SHGFI_LINKOVERLAY = 32768;
        public const uint SHGFI_SELECTED = 65536;
        public const uint SHGFI_ATTR_SPECIFIED = 131072;
        public const uint SHGFI_LARGEICON = 0;
        public const uint SHGFI_SMALLICON = 1;
        public const uint SHGFI_OPENICON = 2;
        public const uint SHGFI_SHELLICONSIZE = 4;
        public const uint SHGFI_PIDL = 8;
        public const uint SHGFI_USEFILEATTRIBUTES = 16;
        public const uint SHGFI_ADDOVERLAYS = 32;
        public const uint SHGFI_OVERLAYINDEX = 64;
        public const uint FILE_ATTRIBUTE_DIRECTORY = 16;
        public const uint FILE_ATTRIBUTE_NORMAL = 128;

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref Shell32.SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        public struct SHITEMID
        {
            public ushort cb;
            [MarshalAs(UnmanagedType.LPArray)]
            public byte[] abID;
        }

        public struct ITEMIDLIST
        {
            public Shell32.SHITEMID mkid;
        }

        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public int lParam;
            public IntPtr iImage;
        }

        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
    }
}