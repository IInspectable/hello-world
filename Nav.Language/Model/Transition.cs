#region Using Directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace Pharmatechnik.Nav.Language {

    sealed class Transition : ITransition {

        internal Transition(TransitionDefinitionSyntax syntax, 
                            ITaskDefinitionSymbol containingTask, 
                            NodeReferenceSymbol source, 
                            EdgeModeSymbol edgeMode, 
                            NodeReferenceSymbol target, 
                            SymbolCollection<TriggerSymbol> triggers)  {

            ContainingTask = containingTask ?? throw new ArgumentNullException(nameof(containingTask));
            Syntax         = syntax         ?? throw new ArgumentNullException(nameof(syntax));
            SourceReference         = source;
            EdgeMode       = edgeMode;
            TargetReference         = target;
            Triggers       = triggers ?? new SymbolCollection<TriggerSymbol>();

            if (source != null) {                
                source.Edge   = this;
            }
            if (edgeMode != null) {
                edgeMode.Edge = this;
            }
            if (target != null) {
                target.Edge = this;
            }
            foreach (var trigger in Triggers) {
                trigger.Transition = this;
            }            
        }

        [NotNull]
        public ITaskDefinitionSymbol ContainingTask { get; }

        [NotNull]
        public Location Location => Syntax.GetLocation();

        [NotNull]
        public TransitionDefinitionSyntax Syntax { get; }
        
        [CanBeNull]
        public INodeReferenceSymbol SourceReference { get; }

        [CanBeNull]
        public IEdgeModeSymbol EdgeMode { get; }

        [CanBeNull]
        public INodeReferenceSymbol TargetReference { get; }
        
        [NotNull]
        public SymbolCollection<TriggerSymbol> Triggers { get; }

        IReadOnlySymbolCollection<ITriggerSymbol> ITransition.Triggers => Triggers;

        [NotNull]
        public IEnumerable<ISymbol> Symbols() {

            if(SourceReference != null) {
                yield return SourceReference;
            }

            if (EdgeMode != null) {
                yield return EdgeMode;
            }

            if (TargetReference != null) {
                yield return TargetReference;
            }

            foreach(var trigger in Triggers) {
                yield return trigger;
            }
        }
    }
}