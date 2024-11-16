﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace CommentLinks
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType(StandardContentTypeNames.Text)]
	[ContentType(StandardContentTypeNames.Projection)]
	[TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
	[TagType(typeof(IntraTextAdornmentTag))]
	internal sealed class CommentLinkAdornmentTaggerProvider : IViewTaggerProvider
	{
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
		[Import]
#pragma warning disable SA1401 // Fields should be private
		internal IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore 649

		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer)
			where T : ITag
		{
			if (textView == null)
			{
				throw new ArgumentNullException("textView");
			}

			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (buffer != textView.TextBuffer)
			{
				return null;
			}

			return CommentLinkAdornmentTagger.GetTagger(
				(IWpfTextView)textView,
				new Lazy<ITagAggregator<CommentLinkTag>>(
					() => this.BufferTagAggregatorFactoryService.CreateTagAggregator<CommentLinkTag>(textView.TextBuffer)))
				as ITagger<T>;
		}
	}
}
