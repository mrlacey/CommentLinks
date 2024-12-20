﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace CommentLinks.Commands
{
	public class CommentLinkCommandBase
	{
		public static readonly Guid CommandSet = new Guid("8b7f9d00-fcc1-4b0f-a951-3d63273c87de");

#pragma warning disable SA1401 // Fields should be private
		protected CommentLinksPackage package;
#pragma warning restore SA1401 // Fields should be private

		protected IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		protected string FormattedLinkText
		{
			get
			{
				switch (this.package.Options.LinkCasing)
				{
					case CaseOption.TitleCase: return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(this.package.Options.TriggerWord);
					case CaseOption.UPPERCASE: return this.package.Options.TriggerWord.ToUpper();
					case CaseOption.lowercase:
					default:
						return this.package.Options.TriggerWord.ToLower();
				}
			}
		}

		protected static string SimpleSpaceEncoding(string original)
		{
			return original.Replace(" ", "%20");
		}

		protected string GetFormattedFilePath(string filePath)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			if (string.IsNullOrWhiteSpace(filePath) | !File.Exists(filePath))
			{
				return null;
			}

			if (this.package.Options.UseAbsolutePaths)
			{
				// Don't change anything. We want the entire file path.
			}
			else
			{
				bool usingLongFileName = false;

				if (this.package.Options.IncludePathToFile)
				{
					var proj = ProjectHelpers.Dte.Solution?.FindProjectItem(filePath)?.ContainingProject;

					// Allow for items that aren't part of a project (such as solution items)
					if (proj != null && !string.IsNullOrWhiteSpace(proj.FileName))
					{
						var projDir = Path.GetDirectoryName(proj.FileName);

						if (filePath.StartsWith(projDir))
						{
							filePath = filePath.Substring(projDir.Length).TrimStart('\\', '/');
							usingLongFileName = true;
						}
					}
				}

				if (!usingLongFileName && !string.IsNullOrWhiteSpace(filePath))
				{
					filePath = Path.GetFileName(filePath);
				}
			}

			return SimpleSpaceEncoding(filePath);
		}
	}
}
