// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommentLinks.Tests
{
	/// <summary>
	/// These tests document scenarios that are not supported in the automatic
	/// detection of filenames when there are subsequent words in the line.
	/// These might otherwise be considered errors.
	/// </summary>
	[TestClass]
	public class ExtractingLinkExceptions
	{
		[TestMethod]
		public void FolderContainsDot_FileNameContainsSpace_WithSubesequentWordsButNoQuotes()
		{
			var sut = CommentLinkTag.Create("My.Helpers/Some File.cs and some other words");

			// Will need to add quotes to get `My.Helpers/Some File.cs`
			Assert.AreEqual("My.Helpers/Some", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);
			Assert.IsNull(sut.SearchTerm);

			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FileWithoutExtension_SubsequentWordsContainHash_WithSubesequentWordsButNoQuotes()
		{
			var sut = CommentLinkTag.Create("README and some # of words");

			// Will need to add quotes to get `README`
			Assert.AreEqual("README and some", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);

			// This isn't null because the hash symbol confused it
			Assert.AreEqual(string.Empty, sut.SearchTerm);

			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FileWithoutExtension_SubsequentWordsContainColon_WithSubesequentWordsButNoQuotes()
		{
			var sut = CommentLinkTag.Create("README in L:25");

			// Will need to add quotes to get `README`
			Assert.AreEqual("README in L", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);

			// This isn't null because the colon confused it
			Assert.AreEqual("25", sut.SearchTerm);

			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FolderContainsDot_FileNameContainsSpace_WithSubesequentWordsContainingHash()
		{
			var sut = CommentLinkTag.Create("My.Helpers/Some File.cs and some other words including # separator");

			// Will need to add quotes to get `My.Helpers/Some File.cs`
			Assert.AreEqual("My.Helpers/Some File.cs and some other words including", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);

			// This isn't null because the hash confused it
			Assert.AreEqual(string.Empty, sut.SearchTerm);

			Assert.IsFalse(sut.IsRunCommand);
		}

		[TestMethod]
		public void FolderContainsDot_FileNameWithoutExtension_WithSubesequentWordsContainingHash()
		{
			var sut = CommentLinkTag.Create("Docs/README and some other words including # separator");

			// Will need to add quotes to get `Docs/README`
			Assert.AreEqual("Docs/README and some other words including", sut.FileName);
			Assert.AreEqual(-1, sut.LineNo);

			// This isn't null because the hash confused it
			Assert.AreEqual(string.Empty, sut.SearchTerm);

			Assert.IsFalse(sut.IsRunCommand);
		}
	}
}
