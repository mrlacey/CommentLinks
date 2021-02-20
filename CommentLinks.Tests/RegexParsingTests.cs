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

        private CommentLinkTag ExtractTagFromLine(string line)
        {
            var regex = RegexHelper.LinkRegex;

            var matches = regex.Matches(line);

            if (matches.Count > 0)
            {
                // Mirroring behavior in link:CommentLinkTagger.cs:ExtractTagFromLine
                return new CommentLinkTag(matches[0].Groups[2].Value);
            }
            else
            {
                return null;
            }
        }
    }
}
