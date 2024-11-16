// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Text.RegularExpressions;

namespace CommentLinks
{
	public class RegexHelper
	{
		public const string LinkMatchPattern = @"((//|\s)link:)(.{4,})";

		public static Regex DefaultLinkRegex { get; }
			= new Regex(
				LinkMatchPattern,
				RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

		public static Regex CreateWithCustomTriggerWord(string triggerword)
		{
			return new Regex(
				LinkMatchPattern.Replace("link", triggerword),
				RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
		}
	}
}
