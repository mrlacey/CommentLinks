// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.Composition;
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
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
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
