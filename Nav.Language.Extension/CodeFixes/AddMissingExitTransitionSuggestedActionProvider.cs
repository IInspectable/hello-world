﻿#region Using Directives

using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using Pharmatechnik.Nav.Language.CodeFixes;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.CodeFixes {

    [ExportCodeFixActionProvider(nameof(AddMissingExitTransitionSuggestedActionProvider))]
    class AddMissingExitTransitionSuggestedActionProvider : CodeFixActionProvider {

        [ImportingConstructor]
        public AddMissingExitTransitionSuggestedActionProvider(CodeFixActionContext context) : base(context) {
        }

        public override IEnumerable<CodeFixSuggestedAction> GetSuggestedActions(CodeFixActionsParameter parameter, CancellationToken cancellationToken) {

            var codeFixes = FindCodeFixes(parameter);

            var actions = codeFixes.Select(codeFix => new AddMissingExitTransitionSuggestedAction(
                codeFix  : codeFix,
                parameter: parameter,
                context  : Context));
         
            return actions;
        }

        public static IEnumerable<AddMissingExitTransitionCodeFix> FindCodeFixes(CodeFixActionsParameter parameter) {

            var targetNodes = parameter.Symbols.OfType<INodeReferenceSymbol>().Where(nr => nr.Type == NodeReferenceType.Target);

            foreach (var targetNode in targetNodes) {

                var taskNode = targetNode.Declaration as ITaskNodeSymbol;
                if (taskNode == null) {
                    continue;
                }

                foreach (var missingExitConnectionPoint in taskNode.GetMissingExitTransitionConnectionPoints()) {
                    var codeFix = new AddMissingExitTransitionCodeFix(parameter.GetEditorSettings(), parameter.CodeGenerationUnitAndSnapshot.CodeGenerationUnit, targetNode, missingExitConnectionPoint);
                    if (codeFix.CanApplyFix()) {
                        yield return codeFix;
                    }
                }
            }           
        }
    }
}