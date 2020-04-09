// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text.Tagging;

namespace CommentLinks
{
    internal class CommentLinkTag : ITag
    {
        internal CommentLinkTag(string link)
        {
            this.Link = link;

            var separatorPos = link.IndexOfAny(new[] { '#', ':' });

            if (separatorPos > 0)
            {
                this.FileName = link.Substring(0, separatorPos);
            }
            else
            {
                this.FileName = link;
            }

            if (separatorPos > -1)
            {
                if (link.Substring(separatorPos).StartsWith("#L"))
                {
                    if (int.TryParse(link.Substring(separatorPos + 2), out int lineNo))
                    {
                        this.LineNo = lineNo;
                    }
                }
                else if (link[separatorPos] == ':')
                {
                    this.SearchTerm = link.Substring(separatorPos + 1);
                }
                else if (link.Substring(separatorPos).StartsWith("#:~:text="))
                {
                    this.SearchTerm = link.Substring(separatorPos + 9);
                }
                else
                {
                    this.SearchTerm = string.Empty;
                }
            }
        }

        public string Link { get; }

        public string FileName { get; }

        public int LineNo { get; } = -1;

        public string SearchTerm { get; }
    }
}
