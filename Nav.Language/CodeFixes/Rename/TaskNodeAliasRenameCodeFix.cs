﻿#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Pharmatechnik.Nav.Language.Text;

#endregion

namespace Pharmatechnik.Nav.Language.CodeFixes.Rename {

    sealed class TaskNodeAliasRenameCodeFix : RenameCodeFix<ITaskNodeAliasSymbol> {
        
        internal TaskNodeAliasRenameCodeFix(ITaskNodeAliasSymbol taskNodeAlias, CodeGenerationUnit codeGenerationUnit, EditorSettings editorSettings) 
            : base(taskNodeAlias, codeGenerationUnit, editorSettings) {
        }

        public override string Name          => "Rename Task Alias";
        public override CodeFixImpact Impact => CodeFixImpact.Medium;
        ITaskDefinitionSymbol ContainingTask => TaskNodeAlias.TaskNode.ContainingTask;
        ITaskNodeAliasSymbol TaskNodeAlias    => Symbol;

        public override bool CanApplyFix() {
            return true;
        }

        public override string ValidateSymbolName(string symbolName) {
            // De facto kein Rename, aber OK
            if (symbolName == TaskNodeAlias.Name) {
                return null;
            }
            return ValidateNewNodeName(symbolName, ContainingTask);            
        }
        
        public override IEnumerable<TextChange> GetTextChanges(string newName) {

            if (!CanApplyFix()) {
                throw new InvalidOperationException();
            }

            newName = newName?.Trim()??String.Empty;

            var validationMessage = ValidateSymbolName(newName);
            if (!String.IsNullOrEmpty(validationMessage)) {
                throw new ArgumentException(validationMessage, nameof(newName));
            }
            
            var textChanges = new List<TextChange?>();
            // Den Task Alias
            textChanges.Add(TryRename(TaskNodeAlias, newName));

            // Die Task-Referenzen auf der "linken Seite"
            foreach (var transition in TaskNodeAlias.TaskNode.Outgoings) {
                var textChange = TryRenameSource(transition, newName);
                textChanges.Add(textChange);
            }

            // Die Task-Referenzen auf der "rechten Seite"
            foreach (var transition in TaskNodeAlias.TaskNode.Incomings) {
                var textChange = TryRenameTarget(transition, newName);
                textChanges.Add(textChange);
            }

            return textChanges.OfType<TextChange>().ToList();
        }
    }
}