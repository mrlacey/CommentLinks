// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommentLinks.Tests
{
    [TestClass]
    public class CustomTriggerWordTests
    {

        [TestMethod]
        public void OneLetterWord_JustFileName()
        {
            var sut = GetFromLineWithCustomTriggerWord("f", "// f:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void TwoLetterWord_JustFileName()
        {
            var sut = GetFromLineWithCustomTriggerWord("go", "// go:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_JustFileName()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "// see:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FourLetterWord_JustFileName()
        {
            var sut = GetFromLineWithCustomTriggerWord("file", "// file:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void FiveLetterWord_JustFileName()
        {
            var sut = GetFromLineWithCustomTriggerWord("magic", "// magic:filename.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_FileNamePlusLineNumber()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "// see:filename.ext#L34");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(34, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_FileNamePlusSimpleSearchTerm()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "// see:filename.ext:FINDME");

            Assert.IsNotNull(sut);
            Assert.AreEqual("filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("FINDME", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_WithCommand_InDoubleQuotes_AndFollowingWords_Valid()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see:run>\"ms-settings: plus this\" but ignore these words");

            Assert.IsNotNull(sut);
            Assert.AreEqual("ms-settings: plus this", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsTrue(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_WithCommand_InSingleQuotes_AndFollowingWords_Valid()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see:run>'ms-settings: plus this' but ignore these words");

            Assert.IsNotNull(sut);
            Assert.AreEqual("ms-settings: plus this", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsTrue(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_WithCommand_InDoubleQuotes_Valid()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see:run>\"ms-settings:\"");

            Assert.IsNotNull(sut);
            Assert.AreEqual("ms-settings:", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsTrue(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_AbsolutePathPlusSearchTerm()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see:C:\\Folder\\filename.ext:FINDME");

            Assert.IsNotNull(sut);
            Assert.AreEqual("C:\\Folder\\filename.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.AreEqual("FINDME", sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_IfOneSpaceBeforeFileNameWithJustExtesion()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see: .ignore");

            Assert.IsNotNull(sut);
            Assert.AreEqual(".ignore", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_FilenameWithJustExtension()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah see:.gitignore");

            Assert.IsNotNull(sut);
            Assert.AreEqual(".gitignore", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        [TestMethod]
        public void ThreeLetterWord_LinkFoundIfATabBeforeLink()
        {
            var sut = GetFromLineWithCustomTriggerWord("see", "blah blah blah\tsee:somefile.ext");

            Assert.IsNotNull(sut);
            Assert.AreEqual("somefile.ext", sut.FileName);
            Assert.AreEqual(-1, sut.LineNo);
            Assert.IsNull(sut.SearchTerm);
            Assert.IsFalse(sut.IsRunCommand);
        }

        private CommentLinkTag GetFromLineWithCustomTriggerWord(string triggerWord, string line)
        {
            var regex = RegexHelper.CreateWithCustomTriggerWord(triggerWord);

            var matches = regex.Matches(line);

            // Mirroring behavior in link:CommentLinkTagger.cs:ExtractTagFromLine
            // Not reusing the actual classes from the extension becuase they have dependencies on VS that are hard to mock/abstract
            if (matches.Count > 0 && matches[0].Groups.Count == 4)
            {
                return CommentLinkTag.Create(matches[0].Groups[3].Value);
            }
            else
            {
                return null;
            }
        }
    }
}
