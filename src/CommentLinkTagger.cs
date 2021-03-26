// Copyright (c) Matt Lacey Ltd. All rights reserved.
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

        protected override IEnumerable<(CommentLinkTag, int)> TryCreateTagsForMatch(Match match)
        {
            if (match.Groups.Count == 4)
            {
                var partAfterKeyword = match.Groups[3].Value;

                var keyword = $"{CommentLinksPackage.Instance?.Options?.TriggerWord}:";

                var submatches = partAfterKeyword.Split(new[] { keyword }, System.StringSplitOptions.RemoveEmptyEntries);

                var indent = match.Value.StartsWith("//") ? 2 : 1;
                var lineOffset = match.Index + indent;

                for (int i = 0; i < submatches.Length; i++)
                {
                    yield return (CommentLinkTag.Create(
                        submatches[i],
                        i == 0 ? indent : 0),
                        lineOffset);
                    lineOffset += submatches[i].Length + keyword.Length;
                }
            }

            yield return (null, 0);
        }
    }
}
