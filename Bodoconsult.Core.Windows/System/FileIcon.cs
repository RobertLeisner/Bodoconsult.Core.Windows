// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Runtime.Versioning;

namespace Bodoconsult.Core.Windows.System
{
    [SupportedOSPlatform("windows")]
    public static class FileIcon
    {

        [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        private static extern IntPtr ExtractIconA(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
        private static extern void DestroyIcon(IntPtr hInst);

        /// <summary>
        /// Dictionary collection that contains file extensions
        /// </summary>
        private static readonly Dictionary<string, Image> Icons = new Dictionary<string, Image>(50);


       
        
        /// <summary>
        /// Get an icon as <see cref="Image"/> for file
        /// </summary>
        /// <param name="filepath">Full path for the file to get the icon for</param>
        /// <returns><see cref="Image"/> object with the icon</returns>
        public static Image GetIcon(string filepath)
        {
            // if specified file path != null and string length > 0 
            if (filepath != null | filepath.Length != 0)
            {
                //get file info of image that can be found with specified filepath
                var file = new FileInfo(filepath);
                // get file extension of image
                var extension = file.Extension.ToLower();

                //if dictionary contains specified file extension -->return extension
                if (Icons.ContainsKey(extension))
                {
                    return Icons[extension];
                }
                // if specified file extension == .dir --> get and return default folder icon
                if (extension == ".dir")
                {
                    GetFolderIcon();
                }
                // if specified exttension neither exist in the dictionary nor matchs with default folder icon --> get and return default unknown file icon
                else
                {
                    GetFileIcon(filepath);
                }
                return GetIcon(extension);
            }
            return null;
        }

        /// <summary>
        /// Gets and adds default folder icon to dictionary
        /// </summary>
        private static void GetFolderIcon()
        {
            // set search flag to file format=icon, to USEFILEATTRIBUTES=yes & icon size = large icons
            var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;
            flags += Shell32.SHGFI_LARGEICON;

            // Get the folder icon from the file information
            var shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(null,
                Shell32.FILE_ATTRIBUTE_DIRECTORY,
                ref shfi,
                (uint)Marshal.SizeOf(shfi),
                flags);

            Icon.FromHandle(shfi.hIcon);	// Load the icon from an HICON handle

            // Now clone the icon, so that it can be successfully stored in an ImageList
            var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();

            DestroyIcon(shfi.hIcon);		// Cleanup
            // add default folder icon to dictionary
            Icons.Add(".dir", icon.ToBitmap());
        }

        /// <summary>
        /// Gets and adds file icon from file that is specified by filepath
        /// </summary>
        /// <param name="filepath">Full path for the file to get the icon for</param>
        private static void GetFileIcon(string filepath)
        {

            var extension = Path.GetExtension(filepath);
            if (extension == null) return;

            var className = Registry.ClassesRoot.OpenSubKey(extension).GetValue("").ToString();

            var server = Registry.ClassesRoot.OpenSubKey(className + @"\DefaultIcon").GetValue("").ToString();


            if (server.Contains(","))
            {
                var i = server.IndexOf(",", StringComparison.Ordinal);

                var exe = server.Substring(0, i);

                var index = Convert.ToInt32(server.Substring(i + 1));


                var p = ExtractIconA(IntPtr.Zero, exe, index);

                var image = Bitmap.FromHicon(p);

                Icons.Add(extension, image);

            }
            else
            {
                Icons.Add(extension, new Bitmap(server));
            }

            //
            //var shfi = new Shell32.SHFILEINFO();
            //// set search flag to file format=icon, to USEFILEATTRIBUTES=yes
            //var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            //// if linkoverlay is set to true add additional flag conditions (SHGFI_LINKOVERLAY)
            //if (linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;
            //flags += Shell32.SHGFI_LARGEICON;  // include the large icon flag

            //// Get the file icon from the file information
            //Shell32.SHGetFileInfo(filepath,
            //    Shell32.FILE_ATTRIBUTE_NORMAL,
            //    ref shfi,
            //    (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
            //    flags);




            //// Now clone the icon, so that it can be successfully stored in an ImageList
            //using (var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone())
            //{
            //    User32.DestroyIcon(shfi.hIcon); // Cleanup
            //    // add default file icon to dictionary
            //    var file = new FileInfo(filepath);
            //    try
            //    {
            //        Icons.Add(file.Extension, icon.ToBitmap());
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(String.Format("Error:GetFileIcon:{0}:{1}", filepath, ex.Message));
            //    }
            //}
        }

        /// <summary>
        /// Get an icon for a certain extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static Image GetIconForExtension(string extension)
        {
            return Icons.ContainsKey(extension) ? Icons[extension] : null;
        }
    }
}
