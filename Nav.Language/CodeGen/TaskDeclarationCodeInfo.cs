﻿#region Using Directives

using System;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    // TODO FROM Factory-Methode einbauen
    public sealed class TaskDeclarationCodeInfo {

        public TaskDeclarationCodeInfo(ITaskDeclarationSymbol taskDeclarationSymbol) {

            if (taskDeclarationSymbol == null) {
                throw new ArgumentNullException(nameof(taskDeclarationSymbol));
            }
            if (taskDeclarationSymbol.IsIncluded) {
                throw new ArgumentException("Only embedded task declarations supported");
            }
           
            Taskname = taskDeclarationSymbol.Name ?? String.Empty;

            if (taskDeclarationSymbol.Origin == TaskDeclarationOrigin.TaskDeclaration) {
                var syntax      = taskDeclarationSymbol.Syntax as TaskDeclarationSyntax;
                NamespacePräfix = syntax?.CodeNamespaceDeclaration?.Namespace?.Text ?? String.Empty;
            } else {
                var syntax      =  taskDeclarationSymbol.Syntax as TaskDefinitionSyntax;
                NamespacePräfix = (syntax?.SyntaxTree.GetRoot() as CodeGenerationUnitSyntax)?.CodeNamespace?.Namespace?.ToString()?? String.Empty;
            }            
        }

        public string Taskname        { get; }
        public string NamespacePräfix { get; }
        public string WflNamespace => $"{NamespacePräfix}.{CodeGenFacts.WflNamespaceSuffix}";
        // TODO TaskName PascalCase?
        public string FullyQualifiedBeginInterfaceName => $"{WflNamespace}.{CodeGenFacts.BeginInterfacePrefix}{Taskname}{CodeGenFacts.WfsClassSuffix}";
    }
}