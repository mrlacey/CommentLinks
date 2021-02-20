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

        private CommentLinkTag ExtractTagFromLine(string line)
        {
            var regex = RegexHelper.LinkRegex;

            var matches = regex.Matches(line);

            if (matches.Count > 0)
            {
                // Mirroring behavior in link:CommentLinkTagger.cs:ExtractTagFromLine
                // Not reusing the actual classes from teh extension becuase they have dependencies on VS that are hard to mock/abstract
                return new CommentLinkTag(matches[0].Groups[2].Value);
            }
            else
            {
                return null;
            }
        }
    }
}
