using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommentLinks.Tests
{
    // Link:CommentLinkTagger.cs finds where to put the button
    // and passes the rest of the line (string) to Link:CommentLinkTag.cs
    [TestClass]
    public class ExtractingLinkTests
    {
        [TestMethod]
        public void JustFileNameWithExtension()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions()
        {
            var sut = CommentLinkTag.Create("SomeFile.Gen.cs");

            Assert.AreEqual("SomeFile.Gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension()
        {
            var sut = CommentLinkTag.Create("Some File.cs");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension()
        {
            var sut = CommentLinkTag.Create("README");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension()
        {
            var sut = CommentLinkTag.Create(".editorconfig");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.gen.cs and some other words");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Some File.cs and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("README and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create(".editorconfig and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25 and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.gen.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Some File.cs andOneOtherWord");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Some File.gen.cs andOneOtherWord");

            Assert.AreEqual("Some File.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("README andOneOtherWord");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create(".editorconfig andOneOtherWord");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25 andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'Some File.cs'");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"README\"");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'.editorconfig'");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Helpers/SomeFile.cs\"");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'My Helpers/SomeFile.cs'");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Cool.Helpers/SomeFile.cs\"");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#L25'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:Find%20me\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#:~:text=Find%20me'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'Some File.cs' and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"README\" and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'.editorconfig' and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'My Helpers/SomeFile.cs' and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Cool.Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#L25' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:Find%20me\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#:~:text=Find%20me' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FolderContainsDot_FileNameWithMutlipleExtension_WithSubesequentWords()
        {
            var sut = CommentLinkTag.Create("/Useful Stuff/Helpers.General.cs and some other words");

            Assert.AreEqual("/Useful Stuff/Helpers.General.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:some search words\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some search words", sut.SearchTerm);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces_AndHaveSubsequentWords()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:some search words\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some search words", sut.SearchTerm);
        }

        [TestMethod]
        public void CanSpecifyAbsoluteFilePathInQuotes()
        {
            var sut = CommentLinkTag.Create(
                "\"C:\\Users\\matt\\Documents\\desktop.ini\"",
                new TestFileSystemAbstraction());

            Assert.AreEqual("C:\\Users\\matt\\Documents\\desktop.ini", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void CanSpecifyAbsoluteFilePathInQuotes_AndHaveSubsequentWords()
        {
            var sut = CommentLinkTag.Create(
                "\"C:\\Users\\matt\\Documents\\desktop.ini\" and some other words",
                new TestFileSystemAbstraction());

            Assert.AreEqual("C:\\Users\\matt\\Documents\\desktop.ini", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveUnderscoreInSearchTerm_WithoutQuotes()
        {
            var sut = CommentLinkTag.Create("cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS");

            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveSemicolonInSearchTerm_WithoutQuotes()
        {
            var sut = CommentLinkTag.Create("cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;");

            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
        }
        /*
         
         
         : someFile.ext#L32
         : somefile.ext:~:~=some words
         : somefile.ext:~:~="some words" > 'some words'
         
         
         */


    }
}
