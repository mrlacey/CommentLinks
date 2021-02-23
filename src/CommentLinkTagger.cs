// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace CommentLinks
{
    internal sealed class CommentLinkTagger : RegexTagger<CommentLinkTag>
    {
        internal CommentLinkTagger(ITextBuffer buffer)
            : base(buffer, new[] { RegexHelper.LinkRegex })
        {
        }

        protected override CommentLinkTag TryCreateTagForMatch(Match match)
        {
            if (match.Groups.Count == 4)
            {
                return CommentLinkTag.Create(match.Groups[3].Value);
            }

            return null;
        }
    }
}
