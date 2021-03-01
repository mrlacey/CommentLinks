// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text.Tagging;

namespace CommentLinks
{
    public class CommentLinkTag : ITag
    {
        private const string TextfragmentIndicator = "#:~:text=";

        private CommentLinkTag()
        {
        }

        public string Link { get; private set; }

        /// <summary>
        /// The name of the file to open.
        /// Or, the command to run if `IsRunCommand` is True.
        /// </summary>
        public string FileName { get; private set; }

        public int LineNo { get; private set; } = -1;

        public string SearchTerm { get; private set; }

        // Position from the start of the RegEx match to show the adornment
        public int Indent { get; private set; }

        public bool IsRunCommand { get; private set; }

        public static CommentLinkTag Create(string link)
        {
            return Create(link, fileSystem: null);
        }

        public static CommentLinkTag Create(string link, IFileSystemAbstraction fileSystem = null)
        {
            return Create(link, 1, fileSystem);
        }

        public static CommentLinkTag Create(string link, int indent = 1, IFileSystemAbstraction fileSystem = null)
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

            if (link.Equals("run>", System.StringComparison.InvariantCultureIgnoreCase))
            {
                // There's no command to execute
                return null;
            }

            var result = new CommentLinkTag
            {
                Link = link,
                Indent = indent,
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

            if (croppedLink.StartsWith("run>", System.StringComparison.InvariantCultureIgnoreCase))
            {
                var command = croppedLink.Substring(4);

                if (command.StartsWith("\""))
                {
                    var nextDblQuoteIndex = command.IndexOf('\"', 1);

                    if (nextDblQuoteIndex >= 0)
                    {
                        command = command.Substring(1, nextDblQuoteIndex - 1);
                    }
                }
                else if (command.StartsWith("'"))
                {
                    var nextSnglQuoteIndex = command.IndexOf('\'', 1);

                    if (nextSnglQuoteIndex >= 0)
                    {
                        command = command.Substring(1, nextSnglQuoteIndex - 1);
                    }
                }
                else
                {
                    var nextSpaceIndex = command.IndexOfAny(new[] { ' ', '\t' });

                    if (nextSpaceIndex >= 0)
                    {
                        command = command.Substring(0, nextSpaceIndex);
                    }
                }

                result.FileName = command;
                result.IsRunCommand = true;
            }
            else
            {
                var separatorPos = croppedLink.IndexOfAny(new[] { '#', ':' });

                if (separatorPos >= 0)
                {
                    if (croppedLink.Length > separatorPos + 1 && croppedLink[separatorPos + 1] == '\\')
                    {
                        separatorPos = croppedLink.IndexOfAny(new[] { '#', ':' }, separatorPos + 1);
                    }
                }

                if (!trailingTextDefinitelyRemoved)
                {
                    if (separatorPos < 0)
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
                else
                {
                    if (separatorPos > 0)
                    {
                        if (croppedLink.Substring(separatorPos).StartsWith(TextfragmentIndicator))
                        {
                            result.SearchTerm = croppedLink.Substring(TextfragmentIndicator.Length + separatorPos);
                        }
                        else if (!croppedLink.Substring(separatorPos).StartsWith("#L"))
                        {
                            // If this adds a search term based on an absolute file path it will be fixed below.
                            result.SearchTerm = croppedLink.Substring(1 + separatorPos);
                        }
                    }
                }

                if (fileSystem.FileExists(croppedLink))
                {
                    result.FileName = croppedLink;
                    separatorPos = -1;  // Reset this as if a valid file path then definitely no search text after a separator
                    result.SearchTerm = null;
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
                    string parseForSearchTerms = null;

                    if (croppedLink.Substring(separatorPos).StartsWith("#L"))
                    {
                        if (int.TryParse(croppedLink.Split(' ')[0].Substring(separatorPos + 2), out int lineNo))
                        {
                            result.LineNo = lineNo;
                        }
                    }
                    else if (croppedLink[separatorPos] == ':')
                    {
                        parseForSearchTerms = croppedLink.Substring(separatorPos + 1);
                    }
                    else if (croppedLink.Substring(separatorPos).StartsWith(TextfragmentIndicator))
                    {
                        parseForSearchTerms = croppedLink.Substring(separatorPos + 9);
                    }
                    else
                    {
                        result.SearchTerm = string.Empty;
                    }

                    if (!string.IsNullOrWhiteSpace(parseForSearchTerms))
                    {
                        parseForSearchTerms = parseForSearchTerms.Trim();

                        var nextSpaceIndex = parseForSearchTerms.IndexOfAny(new[] { ' ', '\t' });

                        if (parseForSearchTerms[0] == '"')
                        {
                            var nextDblQuoteIndex = parseForSearchTerms.IndexOf('"', 1);

                            if (nextDblQuoteIndex >= 0)
                            {
                                result.SearchTerm = parseForSearchTerms.Substring(1, nextDblQuoteIndex - 1);
                            }
                        }
                        else if (parseForSearchTerms[0] == '\'')
                        {
                            var nextSnglQuoteIndex = parseForSearchTerms.IndexOf('\'', 1);

                            if (nextSnglQuoteIndex >= 0)
                            {
                                result.SearchTerm = parseForSearchTerms.Substring(1, nextSnglQuoteIndex - 1);
                            }
                        }

                        if (string.IsNullOrEmpty(result.SearchTerm))
                        {
                            if (nextSpaceIndex >= 0)
                            {
                                result.SearchTerm = parseForSearchTerms.Substring(0, nextSpaceIndex);
                            }
                            else
                            {
                                result.SearchTerm = parseForSearchTerms;
                            }
                        }

                        result.SearchTerm = result.SearchTerm.Replace("%20", " ");
                    }
                }
            }

            return result;
        }
    }
}
