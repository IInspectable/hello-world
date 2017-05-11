﻿#region Using Directives

using System;

using JetBrains.Annotations;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    public sealed class TaskDeclarationCodeModel: CodeModel {

        public TaskDeclarationCodeModel(ITaskDeclarationSymbol taskDeclarationSymbol) {

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

        [NotNull]
        public string Taskname { get; }

        [NotNull]
        public string NamespacePräfix { get; }

        [NotNull]
        public string WflNamespace {
            get { return $"{NamespacePräfix}.WFL"; }
        }

        [NotNull]
        public string FullyQualifiedBeginInterfaceName {
            get { return $"{WflNamespace}.IBegin{Taskname}WFS"; }
        }
    }
}