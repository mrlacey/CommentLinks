﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace CommentLinks
{
	internal sealed class CommentLinkAdornmentTagger
		: IntraTextAdornmentTagger<CommentLinkTag, CommentLinkAdornment>
	{
		private readonly ITagAggregator<CommentLinkTag> tagger;

		private CommentLinkAdornmentTagger(IWpfTextView view, ITagAggregator<CommentLinkTag> tagger)
			: base(view)
		{
			this.tagger = tagger;
		}

		public void Dispose()
		{
			this.tagger.Dispose();

			this.view.Properties.RemoveProperty(typeof(CommentLinkAdornmentTagger));
		}

		internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<CommentLinkTag>> tagger)
		{
			return view.Properties.GetOrCreateSingletonProperty<CommentLinkAdornmentTagger>(
				() => new CommentLinkAdornmentTagger(view, tagger.Value));
		}

		// To produce adornments that don't obscure the text, the adornment tags
		// should have zero length spans. Overriding this method allows control
		// over the tag spans.
		protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, CommentLinkTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
		{
			if (spans.Count == 0)
			{
				yield break;
			}

			ITextSnapshot snapshot = spans[0].Snapshot;

			var clTags = this.tagger.GetTags(spans);

			foreach (IMappingTagSpan<CommentLinkTag> dataTagSpan in clTags)
			{
				NormalizedSnapshotSpanCollection linkTagSpans = dataTagSpan.Span.GetSpans(snapshot);

				// Ignore data tags that are split by projection.
				// This is theoretically possible but unlikely in current scenarios.
				if (linkTagSpans.Count != 1)
				{
					continue;
				}

				SnapshotSpan adornmentSpan = new SnapshotSpan(linkTagSpans[0].Start + dataTagSpan.Tag.Indent, 0);

				yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
			}
		}

		protected override CommentLinkAdornment CreateAdornment(CommentLinkTag dataTag, SnapshotSpan span)
		{
			return new CommentLinkAdornment(dataTag, span.Snapshot.GetLineNumberFromPosition(span.Start));
		}

		protected override bool UpdateAdornment(CommentLinkAdornment adornment, CommentLinkTag dataTag)
		{
			adornment.Update(dataTag);
			return true;
		}
	}
}
