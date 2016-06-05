﻿#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Pharmatechnik.Nav.Language.Extension.Common;
using Pharmatechnik.Nav.Language.Extension.LanguageService;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.HighlightReferences {

    sealed class ReferenceHighlightTagger : SemanticModelServiceDependent, ITagger<TextMarkerTag> {

        [NotNull]
        readonly IDisposable _observable;
        [NotNull]
        readonly List<SnapshotSpan> _referenceSpans;

        public ReferenceHighlightTagger(ITextView view, ITextBuffer textBuffer) : base(textBuffer) {
            View = view;
            
            _referenceSpans = new List<SnapshotSpan>();

            // Wir drosseln hier das Highlighting etwas, um nicht zu viel 
            // Unruhe in die GUI zu bekommen. Der C# Editor verzögert ähnlich.
            _observable = Observable.FromEventPattern<EventArgs>(
                                               handler => Invalidated += handler,
                                               handler => Invalidated -= handler)
                                   .Throttle(ServiceProperties.ReferenceHighlighting)
                                   .ObserveOn(SynchronizationContext.Current)
                                   .Select(   _ => RebuildReferences())
                                   .Subscribe(_ => OnTagsChanged());

            View.Caret.PositionChanged += OnCaretPositionChanged;
            View.LayoutChanged         += OnViewLayoutChanged;
        }
        
        public override void Dispose() {
            base.Dispose();

            _observable.Dispose();

            View.Caret.PositionChanged -= OnCaretPositionChanged;
            View.LayoutChanged         -= OnViewLayoutChanged;
        }

        public ITextView View { get; }

        void OnViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e) {
            // If a new snapshot wasn't generated, then skip this layout 
            if(e.NewSnapshot != e.OldSnapshot) {
                Invalidate();
            }
        }

        protected override void OnSemanticModelChanging(object sender, EventArgs e) {
            Invalidate(clearImmediately: true);
        }

        protected override void OnSemanticModelChanged(object sender, SnapshotSpanEventArgs e) {
            Invalidate();
        }
        
        void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e) {
            Invalidate();
        }

        void Invalidate(bool clearImmediately=false) {

            var point = GetCaretSnapshotPoint();

            // Wenn das Caret nur innerhalb der Referenzen positioniert wurde, und kein Neubau erforderlich ist
            // dann bleibt alles wie es ist.
            if(!clearImmediately && IsPointOverReference(point)) {
                return;
            }
            
            if (clearImmediately || (_referenceSpans.Any() && !IsPointOverReference(point))) {
                // Der Cursor geht aus einer der aktuelle hervorgehobenen Referenz heraus => sofortiges Update,
                // um die Hervorhebung zu entfernen
                _referenceSpans.Clear();
                OnTagsChanged();
            } 

            OnInvalidated();
        }

        void OnInvalidated() {                    
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        event EventHandler<EventArgs> Invalidated;

        void OnTagsChanged() {

            var snapshotSpan = TextBuffer.CurrentSnapshot.GetFullSpan();
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(snapshotSpan));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
        
        public IEnumerable<ITagSpan<TextMarkerTag>> GetTags(NormalizedSnapshotSpanCollection spans) {

            if(spans.Count == 0 || _referenceSpans.Count == 0) {
                yield break;
            }

            // der erste Span hat immer den Charackter der Definition, die übrigen sind dessen Referenzen.
            var definitionSpan = _referenceSpans.First();
            var referenceSpans = new NormalizedSnapshotSpanCollection(_referenceSpans.Skip(1));

            // If the requested snapshot isn't the same as the one our references are on, 
            // translate our spans to the expected snapshot 
            if (spans[0].Snapshot != _referenceSpans[0].Snapshot) {

                referenceSpans = new NormalizedSnapshotSpanCollection(
                    referenceSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

                definitionSpan = definitionSpan.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
            }

            // Die "Definition"
            yield return new TagSpan<TextMarkerTag>(definitionSpan, new DefinitionHighlightTag());

            // Und die zugehörigen Referenzen
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, referenceSpans)) {
                yield return new TagSpan<TextMarkerTag>(span, new ReferenceHighlightTag());
            }           
        }

        List<SnapshotSpan> RebuildReferences() {

            _referenceSpans.Clear();

            var newReferences = BuildReferences(SemanticModelService.SemanticModelResult).ToList();
            if (newReferences.Count > 1) {
                _referenceSpans.AddRange(newReferences);
            }

            return _referenceSpans;
        }

        IEnumerable<SnapshotSpan> BuildReferences(SemanticModelResult semanticModelResult) {

            if(semanticModelResult == null) {
                yield break;
            }

            var point = GetCaretSnapshotPoint();
                     
            if(point == null) {
                yield break;
            }

            if(!semanticModelResult.IsCurrent(point.Value.Snapshot)) {
                yield break;
            }

            var symbol = semanticModelResult.CodeGenerationUnit.Symbols.FindAtPosition(point.Value.Position);

            if (symbol == null && point.Value!= point.Value.GetContainingLine().Start) {
                symbol = semanticModelResult.CodeGenerationUnit.Symbols.FindAtPosition(point.Value.Position-1);
            }

            if (symbol == null) {
                yield break;
            }

            var advancedOptions = NavLanguagePackage.GetAdvancedOptionsDialogPage();
         
            foreach (var reference in ReferenceFinder.FindReferences(symbol, advancedOptions)) {

                yield return new SnapshotSpan(
                    new SnapshotPoint(semanticModelResult.Snapshot, reference.Start), 
                    reference.Location.Length);
            }
        }

        bool IsPointOverReference(SnapshotPoint? point) {

            if (_referenceSpans.Count == 0 || point == null) {
                return false;
            }

            var referenceSpans = new NormalizedSnapshotSpanCollection(_referenceSpans);

            if (_referenceSpans[0].Snapshot != point.Value.Snapshot) {
                referenceSpans = new NormalizedSnapshotSpanCollection(
                    referenceSpans.Select(span => span.TranslateTo(point.Value.Snapshot, SpanTrackingMode.EdgeExclusive)));

            }

            return referenceSpans.Any(r => r.Span.Start <= point.Value.Position && r.Span.End >= point.Value.Position);
        }

        SnapshotPoint? GetCaretSnapshotPoint() {

            var point = View.Caret.Position.Point.GetPoint(TextBuffer, View.Caret.Position.Affinity);
            return point;
        }        
    }
}