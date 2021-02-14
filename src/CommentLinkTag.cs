// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text.Tagging;

namespace CommentLinks
{
    public class CommentLinkTag : ITag
    {
        public CommentLinkTag(string link)
        {
            this.Link = link;

            string croppedLink = link;
            bool trailingTextDefinitelyRemoved = false;

            // handle file names wrapped in single or double quotes
            if (croppedLink.StartsWith("\""))
            {
                var closingIndex = croppedLink.IndexOf('"', 1);
                if (closingIndex > 1)
                {
                    croppedLink = croppedLink.Substring(1, closingIndex - 1);
                    trailingTextDefinitelyRemoved = true;
                }
            }
            else if (croppedLink.StartsWith("'"))
            {
                var closingIndex = croppedLink.IndexOf('\'', 1);
                if (closingIndex > 1)
                {
                    croppedLink = croppedLink.Substring(1, closingIndex - 1);
                    trailingTextDefinitelyRemoved = true;
                }
            }

            var separatorPos = croppedLink.IndexOfAny(new[] { '#', ':' });

            if (!trailingTextDefinitelyRemoved)
            {
                if (separatorPos > 0)
                {
                    // If there's a separator get everything up to the first space after it
                    if (croppedLink.Substring(separatorPos).Contains(" "))
                    {
                        croppedLink = croppedLink.Substring(0, croppedLink.IndexOf(" ", separatorPos));
                    }
                }
                else
                {
                    var firstDot = croppedLink.IndexOf('.');

                    // If there's a '.' assume it's for distinguishing the file name from its extension
                    if (firstDot >= 0)
                    {
                        // Get everything up to the first space after the '.'
                        if (croppedLink.Substring(firstDot).Contains(" "))
                        {
                            croppedLink = croppedLink.Substring(0, croppedLink.IndexOf(" ", firstDot));
                        }
                    }
                    else if (croppedLink.Contains(" "))
                    {
                        // Get everything up to the first space
                        croppedLink = croppedLink.Substring(0, croppedLink.IndexOf(" "));
                    }
                }
            }

            if (System.IO.File.Exists(croppedLink))
            {
                this.FileName = croppedLink;
                separatorPos = -1;  // Reset this as if a valid file path then definitely no search text after a separator
            }
            else
            {
                if (separatorPos > 0)
                {
                    this.FileName = croppedLink.Substring(0, separatorPos);
                }
                else
                {
                    this.FileName = croppedLink;
                }
            }

            this.FileName = this.FileName.Replace("%20", " ").Trim();

            if (separatorPos > -1)
            {
                if (croppedLink.Substring(separatorPos).StartsWith("#L"))
                {
                    if (int.TryParse(croppedLink.Split(' ')[0].Substring(separatorPos + 2), out int lineNo))
                    {
                        this.LineNo = lineNo;
                    }
                }
                else if (croppedLink[separatorPos] == ':')
                {
                    this.SearchTerm = croppedLink.Substring(separatorPos + 1).Replace("%20", " ");
                }
                else if (croppedLink.Substring(separatorPos).StartsWith("#:~:text="))
                {
                    this.SearchTerm = croppedLink.Substring(separatorPos + 9).Replace("%20", " ");
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
