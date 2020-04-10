// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace CommentLinks
{
    internal sealed class CommentLinkTagger : RegexTagger<CommentLinkTag>
    {
        internal CommentLinkTagger(ITextBuffer buffer)
            : base(
                  buffer,
                  new[] { new Regex(@"(link:)([0-9a-z. %\\/\:\#\~\=]{4,})", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase) })
        {
        }

        protected override CommentLinkTag TryCreateTagForMatch(Match match)
        {
            if (match.Groups.Count == 3)
            {
                return new CommentLinkTag(match.Groups[2].Value);
            }

            return null;
        }
    }
}
