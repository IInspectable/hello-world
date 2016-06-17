#region Using Directives

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

using Pharmatechnik.Nav.Language.Extension.Common;
using Pharmatechnik.Nav.Language.Extension.GoToLocation;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.CSharp.GoTo {
    internal sealed class IntraTextGoToAdornmentTagger : IntraTextAdornmentTagger<IntraTextGoToTag, IntraTextGoToAdornment>, IDisposable {
        
        readonly ITagAggregator<IntraTextGoToTag> _goToNavTagger;
        readonly GoToLocationService _goToLocationService;

        IntraTextGoToAdornmentTagger(IWpfTextView textView, ITagAggregator<IntraTextGoToTag> goToNavTagger, GoToLocationService goToLocationService)
            : base(textView) {
            _goToNavTagger = goToNavTagger;
            _goToLocationService = goToLocationService;
            goToNavTagger.TagsChanged += OnTagsChanged;
        }

        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<IntraTextGoToTag>> colorTagger, GoToLocationService goToLocationService) {
            return view.Properties.GetOrCreateSingletonProperty(
                () => new IntraTextGoToAdornmentTagger(view, colorTagger.Value, goToLocationService));
        }

        void OnTagsChanged(object sender, TagsChangedEventArgs e) {
            InvalidateSpans(new List<SnapshotSpan>() {
                TextView.TextBuffer.CurrentSnapshot.GetFullSpan()
            });
        }

        public void Dispose() {
            _goToNavTagger.TagsChanged -= OnTagsChanged;
            _goToNavTagger.Dispose();

            TextView.Properties.RemoveProperty(typeof(IntraTextGoToAdornmentTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, IntraTextGoToTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans) {
            if(spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var colorTags = _goToNavTagger.GetTags(spans);

            foreach(IMappingTagSpan<IntraTextGoToTag> dataTagSpan in colorTags) {
                NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if(colorTagSpans.Count != 1)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].End, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?) PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }

        protected override IntraTextGoToAdornment CreateAdornment(IntraTextGoToTag dataTag, SnapshotSpan span) {
            return new IntraTextGoToAdornment(dataTag, TextView, _goToLocationService);
        }

        protected override bool UpdateAdornment(IntraTextGoToAdornment adornment, IntraTextGoToTag dataTag) {
            adornment.Update(dataTag);
            return true;
        }
    }
}