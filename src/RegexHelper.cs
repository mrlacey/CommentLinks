// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Text.RegularExpressions;

namespace CommentLinks
{
    public class RegexHelper
    {
        public const string LinkMatchPattern = @"(\slink:)([0-9a-z. \'""\-%\\/\:_\;\#\~\=\(\)]{4,})";

        public static Regex LinkRegex { get; } = new Regex(LinkMatchPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
}
