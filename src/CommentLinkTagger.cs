﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace CommentLinks
{
	internal sealed class CommentLinkTagger : RegexTagger<CommentLinkTag>
	{
		internal CommentLinkTagger(ITextBuffer buffer, Regex regex)
			: base(buffer, new[] { regex })
		{
		}

		protected override IEnumerable<(CommentLinkTag, int, int)> TryCreateTagsForMatch(Match match)
		{
			if (match.Groups.Count == 4)
			{
				var partAfterKeyword = match.Groups[3].Value;

				var keyword = $"{CommentLinksPackage.Instance?.Options?.TriggerWord}:";

				var subMatches = partAfterKeyword.Split(new[] { keyword }, System.StringSplitOptions.RemoveEmptyEntries);

				var indent = match.Value.ToLowerInvariant().IndexOf(keyword.ToLowerInvariant());
				var lineOffset = match.Index;

				for (int i = 0; i < subMatches.Length; i++)
				{
					yield return (
						CommentLinkTag.Create(subMatches[i], indent),
						lineOffset,
						subMatches[i].Length);

					lineOffset += subMatches[i].Length + keyword.Length;
				}
			}

			yield return (null, 0, 0);
		}
	}
}
