using System.Collections.Generic;
using System.Linq;

namespace Pharmatechnik.Nav.Language.CodeGen {
    sealed class CodeModelBuilder {
        public static IEnumerable<InitTransitionCodeModel> GetInitTransitions(ITaskDefinitionSymbol taskDefinition, TaskCodeModel taskCodeModel) {
            return taskDefinition.NodeDeclarations
                .OfType<IInitNodeSymbol>().SelectMany(n => n.Outgoings)
                .Select(trans => InitTransitionCodeModel.FromInitTransition(trans, taskCodeModel));
        }

        public static IEnumerable<ExitTransitionCodeModel> GetExitTransitions(ITaskDefinitionSymbol taskDefinition) {
            // TODO Exit Transitions m�ssen pro TaskNode immer zusammengefasst werden
            return taskDefinition.NodeDeclarations
                .OfType<ITaskNodeSymbol>()
                .Select(ExitTransitionCodeModel.FromTaskNode);
        }

        public static IEnumerable<TriggerTransitionCodeModel> GetTriggerTransitions(ITaskDefinitionSymbol taskDefinition) {
            return taskDefinition.NodeDeclarations
                .OfType<IGuiNodeSymbol>()
                .SelectMany(n => n.Outgoings)
                .SelectMany(TriggerTransitionCodeModel.FromTriggerTransition)
                .OrderBy(st => st.TriggerName.Length).ThenBy(st => st.TriggerName);            
        }

        public static IEnumerable<BeginWrapperCodeModel> GetBeginWrappers(ITaskDefinitionSymbol taskDefinition) {
            return taskDefinition.NodeDeclarations
                .OfType<ITaskNodeSymbol>()
                .Select(BeginWrapperCodeModel.FromTaskNode);
        }
    }
}