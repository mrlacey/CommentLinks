// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text.Tagging;

namespace CommentLinks
{
    public class CommentLinkTag : ITag
    {
        private CommentLinkTag()
        {
        }

        public string Link { get; private set; }

        public string FileName { get; private set; }

        public int LineNo { get; private set; } = -1;

        public string SearchTerm { get; private set; }

        public static CommentLinkTag Create(string link, IFileSystemAbstraction fileSystem = null)
        {
            if (fileSystem == null)
            {
                fileSystem = new DefaultFileSystemAbstraction();
            }

            if (string.IsNullOrWhiteSpace(link) || link.StartsWith("  ") || link.StartsWith("\t"))
            {
                return null;
            }

            if (link.StartsWith(" "))
            {
                var linkWithoutStart = link.TrimStart();

                var dotPos = linkWithoutStart.IndexOf('.');
                var spacePos = linkWithoutStart.IndexOf(' ');

                // allow for a space at the start of a possible link if the following looks like a file path/name
                if (dotPos < 0)
                {
                    return null;
                }

                if (spacePos > 0 && spacePos < dotPos)
                {
                    return null;
                }
            }

            link = link.Trim();

            var result = new CommentLinkTag
            {
                Link = link,
            };

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

            if (fileSystem.FileExists(croppedLink))
            {
                result.FileName = croppedLink;
                separatorPos = -1;  // Reset this as if a valid file path then definitely no search text after a separator
            }
            else
            {
                if (separatorPos > 0)
                {
                    result.FileName = croppedLink.Substring(0, separatorPos);
                }
                else
                {
                    result.FileName = croppedLink;
                }
            }

            result.FileName = result.FileName.Replace("%20", " ").Trim();

            if (separatorPos > -1)
            {
                if (croppedLink.Substring(separatorPos).StartsWith("#L"))
                {
                    if (int.TryParse(croppedLink.Split(' ')[0].Substring(separatorPos + 2), out int lineNo))
                    {
                        result.LineNo = lineNo;
                    }
                }
                else if (croppedLink[separatorPos] == ':')
                {
                    result.SearchTerm = croppedLink.Substring(separatorPos + 1).Replace("%20", " ");
                }
                else if (croppedLink.Substring(separatorPos).StartsWith("#:~:text="))
                {
                    result.SearchTerm = croppedLink.Substring(separatorPos + 9).Replace("%20", " ");
                }
                else
                {
                    result.SearchTerm = string.Empty;
                }
            }

            return result;
        }
    }
}
