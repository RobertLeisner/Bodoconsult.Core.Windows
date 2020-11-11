# What does the library

Bodoconsult.Core.Windows provides features related to Microsoft Windows operating system.

Current features are:

+Icon extraction as bitmap
+Reading data from url files (get the included link address in file)


# How to use the library

The source code contain a NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Using FileSystemUrl classes: extract a link address

            // Arrange
            var url = Path.Combine(TestHelper.TestDataPath, "Bodoconsult.url");

            var fri = new FileInfo(url);

            var urlFile = new FileSystemUrl(fri);

            // Act
            urlFile.Read();

            // Assert
            Assert.AreEqual("http://www.bodoconsult.de/", urlFile.Url);
            Assert.AreEqual("Bodoconsult", urlFile.Caption);

## Using IconsAsFilesHelper to get GIF images from an app icon

            var iconDocx = Path.Combine(TestHelper.OutputPath, "docx.gif");
            if (File.Exists(iconDocx)) File.Delete(iconDocx);

            var iconXlsx = Path.Combine(TestHelper.OutputPath, "xlsx.gif");
            if (File.Exists(iconXlsx)) File.Delete(iconXlsx);


            var icons = new IconsAsFilesHelper {IconPath = TestHelper.OutputPath};

            var path = Path.Combine(TestHelper.TestDataPath, "Test.docx");

            var fri = new FileInfo(path);
            icons.AddExtension(fri);


            path = Path.Combine(TestHelper.TestDataPath, "Test.xlsx");

            fri = new FileInfo(path);
            icons.AddExtension(fri);

            icons.SaveIcons();

            Assert.IsTrue(File.Exists(iconDocx));
            Assert.IsTrue(File.Exists(iconXlsx));


# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

