// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using System.Linq;
using System.Text;

namespace Bodoconsult.Core.Windows.FileSystem
{

    /// <summary>
    /// Class for extracting data from files with extension .url
    /// </summary>
    public class FileSystemUrl
    {



        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="fri">Current url file</param>
        public FileSystemUrl(FileSystemInfo fri)
        {
            Path = fri.FullName;
            Caption = fri.Name.Replace(".url", "");
        }


        /// <summary>
        /// Read the data from the file
        /// </summary>
        public void Read()
        {
            var fileStream = new FileStream(Path, FileMode.Open);
            var numArray = new byte[fileStream.Length];
            fileStream.Read(numArray, 0, (int)fileStream.Length);
            foreach (var str in Encoding.ASCII.GetString(numArray).Split('\r', '\n').Where(s => s.StartsWith("URL=")))
                Url = str.Replace("URL=", "");
        }

        /// <summary>
        /// Link address stored in url file
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Caption: Name of the url file without extension
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// Full path to the url file
        /// </summary>
        public string Path { get; }
    }
}
