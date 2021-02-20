using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommentLinks.Tests
{
    [TestClass]
    public class RegexParsingTests
    {
        [TestMethod]
        public void CanHaveUnderscoreInSearchTerm_WithoutQuotes()
        {
            var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveSemicolonInSearchTerm_WithoutQuotes()
        {
            var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveUnderscoreInSearchTerm_WithQuotesAroundSearchTerm()
        {
            var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:\"SMART_LOG_DEVICE_STATISTICS\"");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("\"SMART_LOG_DEVICE_STATISTICS\"", sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveSemicolonInSearchTerm_WithQuotesAroundSearchTerm()
        {
            var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:\"SMART_LOG_DEVICE_STATISTICS;\"");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("\"SMART_LOG_DEVICE_STATISTICS;\"", sut.SearchTerm);
        }


        [TestMethod]
        public void CanHaveUnderscoreInSearchTerm_WithQuotesAroundAll()
        {
            var sut = ExtractTagFromLine("blah blah blah link:\"cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS\"");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
        }

        [TestMethod]
        public void CanHaveSemicolonInSearchTerm_WithQuotesAroundAll()
        {
            var sut = ExtractTagFromLine("blah blah blah link:\"cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;\"");

            Assert.IsNotNull(sut);
            Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
        }

        [TestMethod]
        public void LinkFoundIfASpaceBeforeLink()
        {
            var sut = ExtractTagFromLine("blah blah blah link:somefile.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("somefile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void LinkFoundIfATabBeforeLink()
        {
            var sut = ExtractTagFromLine("blah blah blah\tlink:somefile.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("somefile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void LinkNotFoundIfNoSpaceBeforeLink_Comment()
        {
            var sut = ExtractTagFromLine("blah blah blah //link:somefile.ext");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void LinkNotFoundIfNoSpaceBeforeLink_AsPartOfWord()
        {
            var sut = ExtractTagFromLine("backlink:somefile.ext");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void FindNothing_IfNothingToFind()
        {
            var sut = ExtractTagFromLine("just some words");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void FindNothing_IfEmptyString()
        {
            var sut = ExtractTagFromLine("");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void FindLink_FilenameWithExtension()
        {
            var sut = ExtractTagFromLine("blah blah link:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FindLink_FilenameWithoutExtension()
        {
            var sut = ExtractTagFromLine("blah blah link:README");

            Assert.IsNotNull(sut);
            Assert.AreEqual("README", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FindLink_FilenameWithJustExtension()
        {
            var sut = ExtractTagFromLine("blah blah link:.gitignore");

            Assert.IsNotNull(sut);
            Assert.AreEqual(".gitignore", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void FindLink_IfOneSpaceBeforeFileNameWithExtesion()
        {
            var sut = ExtractTagFromLine("blah blah link: filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        [TestMethod]
        public void DoNotFindLink_IfTwoSpacesBeforeFileName()
        {
            var sut = ExtractTagFromLine("blah blah link:  filename.ext");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void DoNotFindLink_IfTwoSpacesBeforeFileNameWithJust()
        {
            var sut = ExtractTagFromLine("blah blah link:  .ignore");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void DoNotFindLink_IfOneSpaceBeforeFileNameWithNoExtension()
        {
            var sut = ExtractTagFromLine("blah blah link: README");

            Assert.IsNull(sut);
        }

        [TestMethod]
        public void FindLink_IfOneSpaceBeforeFileNameWithJustExtesion()
        {
            var sut = ExtractTagFromLine("blah blah link: .ignore");

            Assert.IsNotNull(sut);
            Assert.AreEqual(".ignore", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
        }

        private CommentLinkTag ExtractTagFromLine(string line)
        {
            var regex = RegexHelper.LinkRegex;

            var matches = regex.Matches(line);

            if (matches.Count > 0)
            {
                // Mirroring behavior in link:CommentLinkTagger.cs:ExtractTagFromLine
                // Not reusing the actual classes from teh extension becuase they have dependencies on VS that are hard to mock/abstract
                return CommentLinkTag.Create(matches[0].Groups[2].Value);
            }
            else
            {
                return null;
            }
        }
    }
}
