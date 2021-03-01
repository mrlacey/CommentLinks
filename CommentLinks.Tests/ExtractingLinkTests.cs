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
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions()
        {
            var sut = CommentLinkTag.Create("SomeFile.Gen.cs");

            Assert.AreEqual("SomeFile.Gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension()
        {
            var sut = CommentLinkTag.Create("Some File.cs");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension()
        {
            var sut = CommentLinkTag.Create("README");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithJustExtension()
        {
            var sut = CommentLinkTag.Create(".editorconfig");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithLineNumber()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithColonSearchText()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.gen.cs and some other words");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Some File.cs and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("README and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create(".editorconfig and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25 and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.gen.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Some File.cs andOneOtherWord");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Some File.gen.cs andOneOtherWord");

            Assert.AreEqual("Some File.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("README andOneOtherWord");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create(".editorconfig andOneOtherWord");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("My Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("Cool.Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#L25 andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs:Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSingleSubsequentWord()
        {
            var sut = CommentLinkTag.Create("SomeFile.cs#:~:text=Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'Some File.cs'");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"README\"");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'.editorconfig'");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Helpers/SomeFile.cs\"");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'My Helpers/SomeFile.cs'");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Cool.Helpers/SomeFile.cs\"");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#L25'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:Find%20me\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#:~:text=Find%20me'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'Some File.cs' and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"README\" and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'.editorconfig' and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'My Helpers/SomeFile.cs' and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"Cool.Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#L25' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:Find%20me\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords_PlusQuotes()
        {
            var sut = CommentLinkTag.Create("'SomeFile.cs#:~:text=Find%20me' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FolderContainsDot_FileNameWithMutlipleExtension_WithSubesequentWords()
        {
            var sut = CommentLinkTag.Create("/Useful Stuff/Helpers.General.cs and some other words");

            Assert.AreEqual("/Useful Stuff/Helpers.General.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:some search words\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some search words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces_AndHaveSubsequentWords()
        {
            var sut = CommentLinkTag.Create("\"SomeFile.cs:some search words\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some search words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
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
            Assert.IsFalse(sut.IsRunCommand);
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
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanHaveUnderscoreInSearchTerm_WithoutQuotes()
        {
            var sut = CommentLinkTag.Create("cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS");

            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanHaveSemicolonInSearchTerm_WithoutQuotes()
        {
            var sut = CommentLinkTag.Create("cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;");

            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetLineNumber_NoQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#L32");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(32, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetFirstSearchWord_NoQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:some words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:\"some\"");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:'some'");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_NoQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:some");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSearchWords_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:\"some words\"");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSearchWords_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:'some words'");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext:\"some words\" other words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text='some words' other words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetFirstSearchWord_TextFragmentAnchor_NoQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text=some words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_TextFragmentAnchor_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text=\"some\"");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_TextFragmentAnchor_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text='some'");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetOnlySearchWord_TextFragmentAnchor_NoQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text=some");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSearchWords_TextFragmentAnchor_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text=\"some words\"");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSearchWords_TextFragmentAnchor_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text='some words'");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_TextFragmentAnchor_DoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text=\"some words\" other words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_TextFragmentAnchor_SingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text='some words' other words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_SpaceBeforeSearchWord()
        {
            var sut = CommentLinkTag.Create(" someFile.ext: some words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_TextFragmentAnchor_SpaceBeforeSearchWord()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text= some words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_SpaceBeforeSearchWordPlusDoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext: \"some more\" words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_TextFragmentAnchor_SpaceBeforeSearchWordPlusDoubleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text= \"some more\" words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_SpaceBeforeSearchWordPlusSingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext: 'some more' words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SpaceBeforeFileName_CanGetSomeSearchWords_TextFragmentAnchor_SpaceBeforeSearchWordPlusSingleQuotes()
        {
            var sut = CommentLinkTag.Create(" someFile.ext#:~:text= 'some more' words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanGetSearchWords_IgnoreSpaceBeforeDoubleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:  \"some more\" words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanGetSearchWords_IgnoreSpaceBeforeSingleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:  'some more' words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some more", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanGetSearchWords_IcludeSpacesInsideDoubleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:  \" some more \" words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual(" some more ", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanGetSearchWords_IncludeSpacesAfterSingleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:  ' some more ' words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual(" some more ", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanNest_SingleQuotesInsideDoubleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:\"'some more' words\" here");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("'some more' words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void CanNest_DoubleQuotesInsideSingleQuotes()
        {
            var sut = CommentLinkTag.Create("someFile.ext:'\"some more\" words' here");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("\"some more\" words", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void SearchTermCanIncludeEmoji()
        {
            // If works with emoji then can assume it works with any character (this is just a test to verify the change from detecting specific characters.)
            var sut = CommentLinkTag.Create("someFile.ext:yay-👍-Yay other words");

            Assert.AreEqual("someFile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("yay-👍-Yay", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }
    }
}
