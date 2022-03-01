// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace CommentLinks
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TagType(typeof(CommentLinkTag))]
    internal sealed class CommentLinkTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer)
            where T : ITag
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (CommentLinksPackage.Instance == null)
            {
                // Try and force load the project if it hasn't already loaded
                // so can access the configured options.
                if (ServiceProvider.GlobalProvider.GetService(typeof(SVsShell)) is IVsShell shell)
                {
                    // IVsPackage package = null;
                    Guid PackageToBeLoadedGuid = new Guid(PackageGuids.guidCommentLinksPackageString);
                    shell.LoadPackage(ref PackageToBeLoadedGuid, out _);
                }
            }

            var keyword = CommentLinksPackage.Instance?.Options?.TriggerWord;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return null;
            }
            else
            {
                var regex = RegexHelper.CreateWithCustomTriggerWord(keyword);

                return buffer.Properties.GetOrCreateSingletonProperty(
                    () => new CommentLinkTagger(buffer, regex)) as ITagger<T>;
            }
        }
    }
}
