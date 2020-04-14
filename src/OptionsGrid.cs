// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace CommentLinks
{
    public class OptionsGrid : DialogPage
    {
        [Category("General")]
        [DisplayName("Include path to file")]
        [Description("If set to true, links generated with the context menu will include the path to the file.")]
        public bool IncludePathToFile { get; set; } = false;

        [Category("General")]
        [DisplayName("Link text case")]
        [Description("How the link indicator will be cased in links generated from the context menu.")]
        public CaseOption LinkCasing { get; set; } = CaseOption.lowercase;


    }

    public enum CaseOption
    {
        TitleCase,
        lowercase,
        UPPERCASE,
    }
}
