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
            var sut = new CommentLinkTag("SomeFile.cs");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions()
        {
            var sut = new CommentLinkTag("SomeFile.Gen.cs");

            Assert.AreEqual("SomeFile.Gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension()
        {
            var sut = new CommentLinkTag("Some File.cs");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension()
        {
            var sut = new CommentLinkTag("README");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension()
        {
            var sut = new CommentLinkTag(".editorconfig");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder()
        {
            var sut = new CommentLinkTag("Helpers/SomeFile.cs");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace()
        {
            var sut = new CommentLinkTag("My Helpers/SomeFile.cs");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot()
        {
            var sut = new CommentLinkTag("Cool.Helpers/SomeFile.cs");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber()
        {
            var sut = new CommentLinkTag("SomeFile.cs#L25");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText()
        {
            var sut = new CommentLinkTag("SomeFile.cs:Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor()
        {
            var sut = new CommentLinkTag("SomeFile.cs#:~:text=Find%20me");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("SomeFile.cs and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("SomeFile.gen.cs and some other words");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("Some File.cs and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("README and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag(".editorconfig and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("My Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("Cool.Helpers/SomeFile.cs and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("SomeFile.cs#L25 and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("SomeFile.cs:Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords()
        {
            var sut = new CommentLinkTag("SomeFile.cs#:~:text=Find%20me and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("SomeFile.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("SomeFile.gen.cs andOneOtherWord");

            Assert.AreEqual("SomeFile.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("Some File.cs andOneOtherWord");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndMultipleExtensions_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("Some File.gen.cs andOneOtherWord");

            Assert.AreEqual("Some File.gen.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("README andOneOtherWord");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag(".editorconfig andOneOtherWord");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("My Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("Cool.Helpers/SomeFile.cs andOneOtherWord");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("SomeFile.cs#L25 andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("SomeFile.cs:Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSingleSubsequentWord()
        {
            var sut = new CommentLinkTag("SomeFile.cs#:~:text=Find%20me andOneOtherWord");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusQuotes()
        {
            var sut = new CommentLinkTag("'Some File.cs'");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"README\"");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusQuotes()
        {
            var sut = new CommentLinkTag("'.editorconfig'");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"Helpers/SomeFile.cs\"");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusQuotes()
        {
            var sut = new CommentLinkTag("'My Helpers/SomeFile.cs'");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"Cool.Helpers/SomeFile.cs\"");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusQuotes()
        {
            var sut = new CommentLinkTag("'SomeFile.cs#L25'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs:Find%20me\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusQuotes()
        {
            var sut = new CommentLinkTag("'SomeFile.cs#:~:text=Find%20me'");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithSpacesAndExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("'Some File.cs' and some other words");

            Assert.AreEqual("Some File.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void JustFileNameWithoutExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"README\" and some other words");

            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithJustExtension_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("'.editorconfig' and some other words");

            Assert.AreEqual(".editorconfig", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolder_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithSpace_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("'My Helpers/SomeFile.cs' and some other words");

            Assert.AreEqual("My Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithExtensionAndFolderWithDot_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"Cool.Helpers/SomeFile.cs\" and some other words");

            Assert.AreEqual("Cool.Helpers/SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithLineNumber_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("'SomeFile.cs#L25' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(25, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithColonSearchText_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs:Find%20me\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FileNameWithTextFragmentAnchor_PlusSubsequentWords_PlusQuotes()
        {
            var sut = new CommentLinkTag("'SomeFile.cs#:~:text=Find%20me' and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("Find me", sut.SearchTerm);
        }

        [TestMethod]
        public void FolderContainsDot_FileNameWithMutlipleExtension_WithSubesequentWords()
        {
            var sut = new CommentLinkTag("/Useful Stuff/Helpers.General.cs and some other words");

            Assert.AreEqual("/Useful Stuff/Helpers.General.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs:some search words\"");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void CanIncludeSearchTermsInQuotesWithoutEscapingSpaces_AndHaveSubsequentWords()
        {
            var sut = new CommentLinkTag("\"SomeFile.cs:some search words\" and some other words");

            Assert.AreEqual("SomeFile.cs", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("some search words", sut.SearchTerm);
        }
    }
}
