﻿#region Using Directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Editor;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.CodeFixes {

    class CodeFixSuggestedActionsSource : SemanticModelServiceDependent, ISuggestedActionsSource {

        readonly ICodeFixActionProviderService _codeFixActionProviderService;
        readonly ITextView _textView;

        ActionSetsWithRange _cachedActionSets;

        public CodeFixSuggestedActionsSource(ITextBuffer textBuffer, ICodeFixActionProviderService codeFixActionProviderService, ITextView textView)
            : base(textBuffer) {
            _codeFixActionProviderService = codeFixActionProviderService;
            _textView = textView;
        }

        public event EventHandler<EventArgs> SuggestedActionsChanged;

        public bool TryGetTelemetryId(out Guid telemetryId) {
            telemetryId = Guid.Empty;
            return false;
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken) {

            var cachedActionSets = _cachedActionSets;

            if (cachedActionSets?.Range == range) {
                return cachedActionSets.SuggestedActionSets;
            }

            return BuildSuggestedActions(range, cancellationToken);
        }

        public Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken) {
            return Task.Factory.StartNew(() => BuildSuggestedActions(range, cancellationToken).Any(), cancellationToken);
        }

        protected override void OnSemanticModelChanged(object sender, SnapshotSpanEventArgs e) {
            base.OnSemanticModelChanged(sender, e);
            _cachedActionSets = null;
            SuggestedActionsChanged?.Invoke(this, EventArgs.Empty);
        }
        
        protected ImmutableList<SuggestedActionSet> BuildSuggestedActions(SnapshotSpan range, CancellationToken cancellationToken) {

            var semanticModelResult = SemanticModelService?.SemanticModelResult;

            if (semanticModelResult == null || !semanticModelResult.IsCurrent(range.Snapshot)) {
                _cachedActionSets = null;
                return ImmutableList<SuggestedActionSet>.Empty;
            }

            var symbols    = FindSymbols(range, semanticModelResult);
            var parameter  = new CodeFixActionsParameter(symbols, semanticModelResult, _textView);
            var actionsets = BuildSuggestedActions(parameter, cancellationToken);

            if (cancellationToken.IsCancellationRequested) {
                return ImmutableList<SuggestedActionSet>.Empty;
            }

            var actionsetsWithRange = new ActionSetsWithRange(range, actionsets);
            _cachedActionSets = actionsetsWithRange;

            return actionsetsWithRange.SuggestedActionSets;
        }

        protected ImmutableList<SuggestedActionSet> BuildSuggestedActions(CodeFixActionsParameter codeFixActionsParameter, CancellationToken cancellationToken) {

            var suggestedActions = _codeFixActionProviderService.GetSuggestedActions(codeFixActionsParameter, cancellationToken);
            return suggestedActions.ToImmutableList();
        }

        static ImmutableList<ISymbol> FindSymbols(SnapshotSpan range, SemanticModelResult semanticModelResult) {
            var extent  = TextExtent.FromBounds(range.Start, range.End);
            var symbols = semanticModelResult.CodeGenerationUnit.Symbols[extent];
            return symbols.ToImmutableList();
        }

        sealed class ActionSetsWithRange {
            public ActionSetsWithRange(SnapshotSpan range, ImmutableList<SuggestedActionSet> suggestedActionSets) {
                Range = range;
                SuggestedActionSets = suggestedActionSets;
            }

            public SnapshotSpan Range { get; }
            public ImmutableList<SuggestedActionSet> SuggestedActionSets { get; }
        }
    }
}