﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

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
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void CanHaveSemicolonInSearchTerm_WithoutQuotes()
		{
			var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;");

			Assert.IsNotNull(sut);
			Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void CanHaveUnderscoreInSearchTerm_WithQuotesAroundSearchTerm()
		{
			var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:\"SMART_LOG_DEVICE_STATISTICS\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void CanHaveSemicolonInSearchTerm_WithQuotesAroundSearchTerm()
		{
			var sut = ExtractTagFromLine("blah blah blah link:cDriveCommands.cpp:\"SMART_LOG_DEVICE_STATISTICS;\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void CanHaveUnderscoreInSearchTerm_WithQuotesAroundAll()
		{
			var sut = ExtractTagFromLine("blah blah blah link:\"cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void CanHaveSemicolonInSearchTerm_WithQuotesAroundAll()
		{
			var sut = ExtractTagFromLine("blah blah blah link:\"cDriveCommands.cpp:SMART_LOG_DEVICE_STATISTICS;\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("cDriveCommands.cpp", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("SMART_LOG_DEVICE_STATISTICS;", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void LinkFoundIfASpaceBeforeLink()
		{
			var sut = ExtractTagFromLine("blah blah blah link:somefile.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("somefile.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void LinkFoundIfATabBeforeLink()
		{
			var sut = ExtractTagFromLine("blah blah blah\tlink:somefile.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("somefile.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void LinkFoundIfNoSpaceBeforeLink_Comment()
		{
			var sut = ExtractTagFromLine("blah blah blah //link:somefile.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("somefile.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
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
		public void FindLink_CommentsImmediatelyBeforeLinkText()
		{
			var sut = ExtractTagFromLine("//link:filename.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_FilenameWithExtension()
		{
			var sut = ExtractTagFromLine("blah blah link:filename.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_FilenameWithoutExtension()
		{
			var sut = ExtractTagFromLine("blah blah link:README");

			Assert.IsNotNull(sut);
			Assert.AreEqual("README", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_FilenameWithJustExtension()
		{
			var sut = ExtractTagFromLine("blah blah link:.gitignore");

			Assert.IsNotNull(sut);
			Assert.AreEqual(".gitignore", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_IfOneSpaceBeforeFileNameWithExtesion()
		{
			var sut = ExtractTagFromLine("blah blah link: filename.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
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
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_AbsolutePath()
		{
			var sut = ExtractTagFromLine("blah blah link:C:\\Folder\\filename.ext");

			Assert.IsNotNull(sut);
			Assert.AreEqual("C:\\Folder\\filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_AbsolutePathInBrackets()
		{
			var sut = ExtractTagFromLine("blah blah link:\"C:\\Folder\\filename.ext\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("C:\\Folder\\filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_AbsolutePathPlusLineNumber()
		{
			var sut = ExtractTagFromLine("blah blah link:C:\\Folder\\filename.ext#L23");

			Assert.IsNotNull(sut);
			Assert.AreEqual("C:\\Folder\\filename.ext", sut.FileName);
			Assert.AreEqual(23, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindLink_AbsolutePathPlusSearchTerm()
		{
			var sut = ExtractTagFromLine("blah blah link:C:\\Folder\\filename.ext:FINDME");

			Assert.IsNotNull(sut);
			Assert.AreEqual("C:\\Folder\\filename.ext", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.AreEqual("FINDME", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunWithoutChevron_DoesNothing()
		{
			// This is too short to be treated like a file name with no extension
			var sut = ExtractTagFromLine("blah blah link:run");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void RunChevron_WithoutCommand_NotValid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void RunChevron_WithoutCommand_EmptySingleQuotes_NotValid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>''");

			Assert.IsNotNull(sut);
			Assert.AreEqual(string.Empty, sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithoutCommand_EmptyDoubleQuotes_NotValid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>\"\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual(string.Empty, sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>ms-settings:");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_InSingleQuotes_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>'ms-settings:'");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_InDoubleQuotes_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>\"ms-settings:\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommandAndValue_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>ms-settings:easeofaccess-highcontrast");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:easeofaccess-highcontrast", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommandAndValue_InSingleQuotes_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>'ms-settings:easeofaccess-highcontrast'");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:easeofaccess-highcontrast", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommandAndValue_InDoubleQuotes_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>\"ms-settings:easeofaccess-highcontrast\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:easeofaccess-highcontrast", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_AndFollowingWords_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>ms-settings: but ignore these words");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings:", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_InSingleQuotes_AndFollowingWords_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>'ms-settings: plus this' but ignore these words");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings: plus this", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void RunChevron_WithCommand_InDoubleQuotes_AndFollowingWords_Valid()
		{
			var sut = ExtractTagFromLine("blah blah link:run>\"ms-settings: plus this\" but ignore these words");

			Assert.IsNotNull(sut);
			Assert.AreEqual("ms-settings: plus this", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsTrue(sut.IsRunCommand);
		}

		[TestMethod]
		public void SearchAboveInSameFile_WithNoExtraDetails()
		{
			var sut = ExtractTagFromLine("// link:^");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void SearchAboveInSameFile_WithLineNumber()
		{
			var sut = ExtractTagFromLine("// link:^#L100");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void SearchAboveInSameFile_WithSearchTerm()
		{
			var sut = ExtractTagFromLine("// link:^:something");

			Assert.IsNotNull(sut);
			Assert.AreEqual("^", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNotNull(sut.SearchTerm);
			Assert.AreEqual("something", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void SearchAboveInSameFile_WithSearchTermInQuotes()
		{
			var sut = ExtractTagFromLine("// link:^:\"some thing\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("^", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNotNull(sut.SearchTerm);
			Assert.AreEqual("some thing", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void SearchBelowInSameFile_WithNoExtraDetails()
		{
			var sut = ExtractTagFromLine("// link:v");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void SearchBelowInSameFile_WithLineNumber()
		{
			var sut = ExtractTagFromLine("// link:v#L100");

			Assert.IsNull(sut);
		}

		[TestMethod]
		public void SearchBelowInSameFile_WithSearchTerm()
		{
			var sut = ExtractTagFromLine("// link:v:something");

			Assert.IsNotNull(sut);
			Assert.AreEqual("v", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNotNull(sut.SearchTerm);
			Assert.AreEqual("something", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void SearchBelowInSameFile_WithSearchTermInQuotes()
		{
			var sut = ExtractTagFromLine("// link:v:\"some thing\"");

			Assert.IsNotNull(sut);
			Assert.AreEqual("v", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNotNull(sut.SearchTerm);
			Assert.AreEqual("some thing", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_ItemPath()
		{
			var sut = ExtractTagFromLine("// link:$(ItemPath)");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(ItemPath)", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_ItemDir()
		{
			var sut = ExtractTagFromLine("// link:$(ItemDir)");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(ItemDir)", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_ProjectDir()
		{
			var sut = ExtractTagFromLine("// link:$(ProjectDir)");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(ProjectDir)", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_SolutionDir()
		{
			var sut = ExtractTagFromLine("// link:$(SolutionDir)");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(SolutionDir)", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_ItemPath_PlusLineNumber()
		{
			var sut = ExtractTagFromLine("// link:$(ItemPath)#L25");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(ItemPath)", sut.FileName);
			Assert.AreEqual(25, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FindPlaceholder_ItemPath_PlusSearchTerm()
		{
			var sut = ExtractTagFromLine("// link:$(ItemPath):findMe");

			Assert.IsNotNull(sut);
			Assert.AreEqual("$(ItemPath)", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNotNull(sut.SearchTerm);
			Assert.AreEqual("findMe", sut.SearchTerm);
			Assert.IsFalse(sut.IsRunCommand);
		}

		private CommentLinkTag ExtractTagFromLine(string line)
		{
			var regex = RegexHelper.DefaultLinkRegex;

			var matches = regex.Matches(line);

			// Mirroring behavior in link:CommentLinkTagger.cs:ExtractTagFromLine
			// Not reusing the actual classes from the extension because they have dependencies on VS that are hard to mock/abstract
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
